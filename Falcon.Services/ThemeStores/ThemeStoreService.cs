using System;
using System.Collections.Generic;
using System.Linq;
using Falcon.Data.Domain;
using Falcon.Data.Repository;
using Falcon.Services.ThemeColletions;
using Falcon.Services.ThemeFeatures;

namespace Falcon.Services.ThemeStores
{
    public class ThemeStoreService : BaseService, IThemeStoreService
    {
        private readonly IThemeStoreRepository _themeStoreRepository;
        private readonly IThemeFeatureService _themeFeatureService;
        private readonly IThemeColletionService _themeColletionService;
        public ThemeStoreService(IThemeStoreRepository themeStoreRepository, IThemeFeatureService themeFeatureService, IThemeColletionService themeColletionService)
        {
            _themeStoreRepository = themeStoreRepository;
            _themeFeatureService = themeFeatureService;
            _themeColletionService = themeColletionService;
        }


        public ThemeStore GetThemeById(int themeId)
        {
            return _themeStoreRepository.Table.FirstOrDefault(t => t.Id == themeId);
        }

        public ThemeStore GetThemeBySeoUrl(string seoUrl)
        {
            return _themeStoreRepository.Table.FirstOrDefault(t => t.SeoUrl.Contains(seoUrl));
        }

        public IEnumerable<ThemeStore> GetThemeByName(string name)
        {
            return _themeStoreRepository.Table.Where(t => t.Name.Contains(name));
        }

        public IEnumerable<ThemeStore> GetThemeByQuery(string[] query)
        {
            var list = _themeStoreRepository.Table.ToList();
            query = StringQuery(query);

            if (query[0] + "" != "")
            {
                switch (query[0])
                {
                    case "free":
                        list = list.Where(t => t.Price == 0 || t.Price == null).ToList();
                        break;
                    case "paid":
                        list = list.Where(t => t.Price > 0 && t.Price != null).ToList();
                        break;
                    default:
                        break;
                }
                //return list;
            }
            if (query[1] + "" != "")
            {
                try
                {
                    var id = int.Parse(query[1]);
                    var f = _themeFeatureService.GetByFeatureId(id).ToList();
                    var model = f.Select(themeFeature => list.FirstOrDefault(m => m.Id == themeFeature.ThemeId)).Where(item => item != null).ToList();
                    list = model;
                }
                catch (Exception)
                {
                    //throw;
                }

            }
            if (query[2] + "" != "")
            {
                try
                {
                    var id = int.Parse(query[2]);
                    var f = _themeColletionService.GetByColletionId(id).ToList();
                    var model = f.Select(themeFeature => list.FirstOrDefault(m => m.Id == themeFeature.ThemeId)).Where(item => item != null).ToList();
                    list = model;
                }
                catch (Exception)
                {
                    //throw;
                }

            }
            if (query[3] + "" != "")
            {
                try
                {
                    var id = int.Parse(query[3]);
                    switch (id)
                    {
                        case 1:
                            list = list.OrderBy(m => m.Price).ToList();
                            break;
                        case 2:
                            list = list.OrderBy(m => m.Name).ToList();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    //throw;
                }

            }
            return list;
        }

        public IEnumerable<ThemeStore> GetAll()
        {
            return _themeStoreRepository.Table.ToList();
        }

        private string[] StringQuery(string[] str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] + "" == "") continue;
                var s = str[i];
                var k = s.LastIndexOf(",", System.StringComparison.Ordinal);
                if (k > -1)
                {
                    str[i] = s.Substring(k + 1);
                }
            }
            return str;
        }
    }
}
