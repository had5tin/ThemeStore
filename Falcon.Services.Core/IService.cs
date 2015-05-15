using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Falcon.Caching;

namespace Falcon.Services
{
    /// <summary>
    /// Base Interface for all services
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Trả về kết nối đang được Linq sử dụng
        /// </summary>
        /// <returns></returns>
        //IDbConnection GetOpenConnection();

        /// <summary>
        /// Cache hệ thống (Memory Cache)
        /// </summary>
        ICacheManager Cache { get; set; }
    }
}
