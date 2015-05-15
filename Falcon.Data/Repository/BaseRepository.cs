using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data.Repository;
using Falcon.Data.Domain;
using System.Data;
using Dapper;

namespace Falcon.Data.Repository
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly Database _database;

        protected BaseRepository(Database database)
        {
            _database = database;
        }

        protected BaseRepository(IDatabaseFactory factory)
            : this(factory.GetDatabase())
        {
        }

        public Database Database
        {
            get
            {
                return _database;
            }
        }

        public void Add(TEntity entity)
        {
            // _database.GetTable<TEntity>().InsertOnSubmit(entity);
            // _database.SubmitChanges();

            _database.Set<TEntity>().Add(entity);
            _database.SaveChanges();


        }

        public void Remove(TEntity entity)
        {
            //_database.GetTable<TEntity>().DeleteOnSubmit(entity);
            _database.Set<TEntity>().Remove(entity);
            _database.SaveChanges();
        }

        public void SubmitChanges()
        {
            _database.SaveChanges();
        }


        public IQueryable<TEntity> Table
        {
            get
            {
                return _database.Set<TEntity>();
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return _Query<T>(sql, param);
        }

        public IEnumerable<T> QuerySP<T>(string sql, object param = null)
        {
            return _Query<T>(sql, param, CommandType.StoredProcedure);
        }

        public int Execute(string sql, object param = null)
        {
            return _Execute(sql, param);
        }

        public int ExecuteSP(string sql, object param = null)
        {
            return _Execute(sql, param, CommandType.StoredProcedure);
        }

        private IEnumerable<T> _Query<T>(string sql, object param = null, CommandType? commandType = null)
        {
            var conn = _database.Database.Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var result = conn.Query<T>(sql, param, commandType: commandType);
            conn.Close();

            return result;
        }
        private int _Execute(string sql, object param = null, CommandType? commandType = null)
        {
            var conn = _database.Database.Connection;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            var result = conn.Execute(sql, param, commandType: commandType);
            conn.Close();

            return result;
        }
    }
}
