using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(Database database)
            : base(database)
        {            
        }

        public UserRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}
