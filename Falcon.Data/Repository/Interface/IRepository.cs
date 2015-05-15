using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Falcon.Data.Repository
{
    /// <summary>
    /// Repository
    /// </summary>
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {
        /// <summary>
        /// Add new Entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// Remove Entity
        /// </summary>
        /// <param name="entity"></param>
        void Remove(TEntity entity);

        /// <summary>
        /// Commit all change to database
        /// </summary>
        void SubmitChanges();
        IQueryable<TEntity> Table { get; }

        Database Database { get; }

        /// <summary>
        /// Run query return single or collection of result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> Query<T>(string sql, object param = null);

        /// <summary>
        /// Run Store Procedure return single or collection of result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storeProcedureName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        IEnumerable<T> QuerySP<T>(string storeProcedureName, object param = null);

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        int Execute(string sql, object param = null);

        /// <summary>
        /// Execute Store Procedure command
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <param name="param"></param>
        int ExecuteSP(string storeProcedureName, object param = null);
    }
}
