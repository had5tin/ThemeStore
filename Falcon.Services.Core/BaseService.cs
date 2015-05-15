using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StackExchange.Profiling;
using System.Data.Common;
using System.Configuration;
using Falcon.Caching;
using Falcon.Infrastructure;
using Falcon.Data;
using Dapper;

namespace Falcon.Services
{
    public abstract class BaseService : IService
    {
        private ICacheManager cache;
        //private ICacheManager cache = EngineContext.Current.Resolve<ICacheManager>(CacheTypeEnum.Memory.ToString());
        //private ICacheManager cacheRedis = EngineContext.Current.Resolve<ICacheManager>(CacheTypeEnum.Redis.ToString());
        //private ICacheManager cachePerRequest = EngineContext.Current.Resolve<ICacheManager>(CacheTypeEnum.PerRequest.ToString());

        public ICacheManager Cache
        {
            get
            {
                if (cache == null)
                {
                    string cacheProvider = ConfigurationManager.AppSettings["CacheProvider"];

                    if (string.IsNullOrEmpty(cacheProvider) || !Enum.IsDefined(typeof(CacheTypeEnum), cacheProvider))
                    {
                        cacheProvider = CacheTypeEnum.Memory.ToString();
                    }

                    cache = EngineContext.Current.Resolve<ICacheManager>(cacheProvider);
                }
                return cache;
            }
            set
            {
                cache = value;
            }
        }

        /// <summary>
        /// Sẽ bị bắn exception nếu ở 1 đoạn code nào đó sử dụng 
        /// using (var conn = GetOpenConnection()) {
        ///     //thao tác dữ liệu
        /// }
        /// Nguyên nhân: sau đoạn code này connection sẽ bị dispose, các service khác
        /// sử dụng connection này sẽ dính lỗi. Tuy nhiên vẫn để exception này xảy ra
        /// để phát hiện sai sót trong quá trình lập trình
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetOpenConnection()
        {
            var conn = EngineContext.Current.Resolve<IDatabaseFactory>().GetDatabase().Database.Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }            
            return conn;
        }

        public virtual List<T> GetByListId<T>(IEnumerable<int> ids, string tableName)
        {
            var tmp = new DataTable();
            tmp.Columns.Add("Id", typeof(int));
            foreach (var item in ids)
            {
                var row = tmp.NewRow();
                row["Id"] = item;
                tmp.Rows.Add(row);
            }

            var conn = GetOpenConnection();
            var result = conn.Query<T>("GetByListId", new { TableName = tableName, Ids = tmp.AsTableValuedParameter("ListId") }, commandType: CommandType.StoredProcedure).ToList();
            conn.Close();

            return result;
        }
    }
}
