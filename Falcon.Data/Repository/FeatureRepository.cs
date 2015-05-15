using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class FeatureRepository : BaseRepository<Feature>, IFeatureRepository
    {
        public FeatureRepository(Database database)
            : base(database)
        {
        }

        public FeatureRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}
