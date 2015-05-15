using System.Collections.Generic;
using System.Linq;
using Falcon.Data.Domain;
using Falcon.Data.Repository;

namespace Falcon.Services.ThemeFeatures
{
    public class ThemeFeatureService : BaseService, IThemeFeatureService
    {
        private readonly IThemeFeatureRepository _themeFeatureRepository;

        public ThemeFeatureService(IThemeFeatureRepository themeFeatureRepository)
        {
            _themeFeatureRepository = themeFeatureRepository;
        }


        public IEnumerable<ThemeFeature> GetAll()
        {
            //var conn = GetOpenConnection();
            //IEnumerable<Feature> result;
            //result = conn.Query<Feature>("Select Id,UserName from Users");
            //conn.Close();
            //return result;
            return _themeFeatureRepository.Table.ToList();
        }

        public IEnumerable<ThemeFeature> GetByFeatureId(int featureId)
        {
            return _themeFeatureRepository.Table.Where(f => f.FeatureId == featureId);
        }

        public IEnumerable<ThemeFeature> GetByThemeId(int themeId)
        {
            return _themeFeatureRepository.Table.Where(f => f.ThemeId == themeId);
        }
    }
}
