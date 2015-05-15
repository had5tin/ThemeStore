using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class ResourceRepository : BaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(Database database)
            : base(database)
        {
        }

        public ResourceRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
