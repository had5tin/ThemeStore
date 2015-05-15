using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Repository;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class PageRepository : BaseRepository<StaticPage>, IPageRepository
    {
        public PageRepository(Database database)
            : base(database)
        {
        }

        public PageRepository(IDatabaseFactory factory)
            : base(factory)
        {
        }
    }
}
