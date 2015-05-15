using System.Collections.Generic;
using Falcon.Data.Domain;

namespace Falcon.Services.ThemeStores
{
    public interface IThemeStoreService : IService
    {
        ThemeStore GetThemeById(int themeId);
        ThemeStore GetThemeBySeoUrl(string seoUrl);

        IEnumerable<ThemeStore> GetThemeByName(string name);

        IEnumerable<ThemeStore> GetThemeByQuery(string[] query);
        IEnumerable<ThemeStore> GetAll();

    }
}
