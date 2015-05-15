using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class ThemeStoreRepository : BaseRepository<ThemeStore>, IThemeStoreRepository
    {
        public ThemeStoreRepository(Database database)
            : base(database)
        {
        }

        public ThemeStoreRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}
