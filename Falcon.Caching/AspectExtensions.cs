using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Falcon.Data;

namespace Falcon.Caching
{
    public static class AspectExtensions
    {
        #region CacheExtensions
        [DebuggerStepThrough]
        public static AspectF Cache<TReturnType>(this AspectF aspect,
            ICacheManager cacheResolver, string key, int cacheTime = 0)
        {
            return aspect.Combine((work) =>
            {
                Cache<TReturnType>(aspect, cacheResolver, key, cacheTime, work, cached => cached);
            });
        }

        /// <summary>
        /// Cache list item, chỉ lưu Id của các Item, từ danh sách Id đó sẽ lấy chi tiết từng item trong cache ra, 
        /// nếu ko có thì lấy từ database
        /// </summary>
        /// <typeparam name="TItemType"></typeparam>
        /// <param name="aspect"></param>
        /// <param name="cacheResolver"></param>
        /// <param name="listCacheKey"></param>
        /// <param name="cacheTime">Thời gian cache, tính theo giây, mặc định = 0 là không bị hết hạn</param>
        /// <param name="getItemKey"></param>
        /// <param name="getByListId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static AspectF CacheList<TItemType>(this AspectF aspect,
            ICacheManager cacheResolver, string listCacheKey, Func<TItemType, string> getItemKey, Func<List<int>, List<TItemType>> getByListId, int cacheTime = 0)
            where TItemType : BaseEntity
        {
            return aspect.Combine((work) =>
            {
                var workDelegate = aspect.WorkDelegate as Func<List<TItemType>>;

                // Thay work delegate hiện tại bằng work delegate mới để xử lý lấy dữ liệu chi tiết & lưu cache item
                Func<List<TItemType>> newWorkDelegate = () =>
                {
                    List<TItemType> collection = workDelegate();

                    var cacheKeys = new List<string>();
                    var dicCacheKeys = new Dictionary<string, int>();
                    var dicIds = new Dictionary<int, string>();
                    foreach (TItemType item in collection)
                    {
                        string key = getItemKey(item);
                        cacheKeys.Add(key);
                        dicCacheKeys.Add(key, item.Id);
                        dicIds.Add(item.Id, key);
                    }

                    var cachedData = cacheResolver.MGet<TItemType>(cacheKeys);
                    var newIds = cachedData.Where(i => i.Value == null).Select(item => dicCacheKeys[item.Key]).ToList();

                    if (newIds.Count > 0)
                    {
                        var newData = getByListId(newIds);
                        foreach (var item in newData)
                        {
                            string key = getItemKey(item);
                            cachedData[key] = item;

                            cacheResolver.Set(key, item);
                        }
                    }

                    return collection.Select(item => cachedData[dicIds[item.Id]]).ToList();
                };
                aspect.WorkDelegate = newWorkDelegate;

                // Get the collection from cache or real source. If collection is returned
                // from cache, resolve each item in the collection from cache
                Cache<List<TItemType>>(aspect, cacheResolver, listCacheKey, cacheTime, work,
                    cached =>
                    {
                        // Kiểm tra nếu có 1 item nào đó không tồn tại trong cache thì lấy lại cache từ đầu
                        var cacheKeys = cached.Select(getItemKey).ToList();

                        if (cacheResolver.MExists(cacheKeys).Any(exist => exist == false))
                        {
                            return default(List<TItemType>);
                        }

                        return cached;
                    });
            });
        }

        [DebuggerStepThrough]
        public static AspectF CacheRetry<TReturnType>(this AspectF aspect,
            ICacheManager cacheResolver,
            string key, int cacheTime)
        {
            return aspect.Combine((work) =>
            {
                try
                {
                    Cache<TReturnType>(aspect, cacheResolver, key, cacheTime, work, cached => cached);
                }
                catch (Exception x)
                {
                    //logger.LogException(x);
                    System.Threading.Thread.Sleep(1000);

                    //Retry
                    try
                    {
                        Cache<TReturnType>(aspect, cacheResolver, key, cacheTime, work, cached => cached);
                    }
                    catch (Exception ex)
                    {
                        //logger.LogException(ex);
                        throw ex;
                    }
                }
            });
        }

        private static void Cache<TReturnType>(AspectF aspect, ICacheManager cacheResolver,
            string key, int cacheTime, Action work, Func<TReturnType, TReturnType> foundInCache)
        {
            var cachedData = cacheResolver.Get<TReturnType>(key);
            if (cachedData == null)
            {
                GetListFromSource<TReturnType>(aspect, cacheResolver, key, cacheTime);
            }
            else
            {
                // Give caller a chance to shape the cached item before it is returned
                TReturnType cachedType = foundInCache(cachedData);
                if (cachedType == null)
                {
                    GetListFromSource<TReturnType>(aspect, cacheResolver, key, cacheTime);
                }
                else
                {
                    aspect.WorkDelegate = new Func<TReturnType>(() => cachedType);
                }
            }

            work();
        }

        private static void GetListFromSource<TReturnType>(AspectF aspect, ICacheManager cacheResolver, string key, int cacheTime)
        {
            var workDelegate = aspect.WorkDelegate as Func<TReturnType>;
            TReturnType realObject = workDelegate();
            cacheResolver.Set(key, realObject, cacheTime);
            workDelegate = () => realObject;
            aspect.WorkDelegate = workDelegate;
        }
        #endregion
    }
}
