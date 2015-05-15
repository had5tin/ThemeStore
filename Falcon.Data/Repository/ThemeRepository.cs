using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class ThemeRepository : BaseRepository<Theme>, IThemeRepository
    {
        public ThemeRepository(Database database)
            : base(database)
        {            
        }

        public ThemeRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}
