using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Falcon.Services
{
    internal static class Extensions
    {
        internal static Dapper.SqlMapper.ICustomQueryParameter ToTVP(this IEnumerable<int> listId, string typeName = "ListId")
        {
            var tmp = new DataTable();
            tmp.Columns.Add("Id", typeof(int));
            foreach (var item in listId)
            {
                var row = tmp.NewRow();
                row["Id"] = item;
                tmp.Rows.Add(row);
            }
            return tmp.AsTableValuedParameter(typeName);
        }
    }
}
