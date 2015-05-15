﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
    public class ThemeFeatureRepository : BaseRepository<ThemeFeature>, IThemeFeatureRepository
    {
        public ThemeFeatureRepository(Database database)
            : base(database)
        {
        }

        public ThemeFeatureRepository(IDatabaseFactory factory)
            : base(factory.GetDatabase())
        {
        }
    }
}