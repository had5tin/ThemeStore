using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(Database database)
            : base(database)
        {
        }

        public PermissionRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
