using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(Database database)
            : base(database)
        {
        }

        public UserRoleRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
