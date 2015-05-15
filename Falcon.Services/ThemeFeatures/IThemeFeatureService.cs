using System.Collections.Generic;
using Falcon.Data.Domain;

namespace Falcon.Services.ThemeFeatures
{
    public interface IThemeFeatureService : IService
    {
        IEnumerable<ThemeFeature> GetAll();
        IEnumerable<ThemeFeature> GetByFeatureId(int featureId);
        IEnumerable<ThemeFeature> GetByThemeId(int themeId);
    }
}
