using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class CollectionRepository : BaseRepository<Collection>, ICollectionRepository
    {
        public CollectionRepository(Database database)
            : base(database)
        {
        }

        public CollectionRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}
