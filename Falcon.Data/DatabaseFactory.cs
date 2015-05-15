using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using StackExchange.Profiling;

namespace Falcon.Data
{
    public class DatabaseFactory :  IDatabaseFactory
    {
        private readonly string _connectionString;
        private Database _database;

        public DatabaseFactory()
        {
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }

        public Database GetDatabase()
        {
            if (_database == null)
            {
                _database = new Database(_connectionString);
            }
            return _database;
        }
    }
}
