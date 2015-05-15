using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(Database database)
            : base(database)
        {
        }

        public RoleRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
