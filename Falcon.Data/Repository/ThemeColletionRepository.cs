using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class ThemeColletionRepository : BaseRepository<ThemeColletion>, IThemeColletionRepository
    {
        public ThemeColletionRepository(Database database)
            : base(database)
        {
        }

        public ThemeColletionRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}
