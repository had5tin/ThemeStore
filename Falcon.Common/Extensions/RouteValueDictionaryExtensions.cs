using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace Falcon.Common.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        /// <summary>
        /// Tạo một đối tượng mới giống đối tượng cũ
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static RouteValueDictionary Clone(this RouteValueDictionary route)
        {
            if (route != null)
            {
                return new RouteValueDictionary(route);
            }
            else
            {
                return new RouteValueDictionary();
            }
        }

        /// <summary>
        /// Sắp xếp
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static RouteValueDictionary CloneSortByKey(this RouteValueDictionary route)
        {
            if (route != null)
            {
                var dic = route.OrderBy(a => a.Key).ToDictionary(a => a.Key, v => v.Value);
                var result = new RouteValueDictionary(dic);
                return result;
            }
            else
            {
                return new RouteValueDictionary();
            }
        }

        public static RouteValueDictionary CloneSortByKey(this RouteValueDictionary route, Dictionary<string, object> source)
        {
            if (route != null)
            {
                var dic = source.Union(route.OrderBy(a => a.Key)).ToDictionary(a => a.Key, v => v.Value);
                var result = new RouteValueDictionary(dic);
                return result;
            }
            else
            {
                return new RouteValueDictionary();
            }
        }

        /// <summary>
        /// Thực hiện 1 thao tác lên đối tượng, trả về chính đối tượng đó
        /// </summary>
        /// <param name="route"></param>
        /// <param name="action"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RouteValueDictionary Action(this RouteValueDictionary route, string action, string key, object value = null)
        {
            if ("Add".Equals(action))
            {
                route.Add(key, value);
            }
            else if ("Remove".Equals(action))
            {
                route.Remove(key);
            }
            else if ("Set".Equals(action))
            {
                if (route[key] != null)
                {
                    route[key] = value;
                }
                else
                {
                    route.Add(key, value);
                }
            }
            return route;
        }
    }
}
