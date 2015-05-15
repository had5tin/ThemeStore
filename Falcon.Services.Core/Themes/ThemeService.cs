using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Themes;
using Falcon.Data.Repository;
using Falcon.Data.Domain;
using Falcon.Caching;

namespace Falcon.Services.Themes
{
    public class ThemeService : BaseService, IThemeService
    {
        private readonly IThemeRepository _themeRepository;

        public ThemeService(IThemeRepository themeRepository)
        {
            this._themeRepository = themeRepository;
        }


        public string GetTheme(ThemeType themeType = ThemeType.Portal)
        {
            var theme = AspectF.Define
                .Cache<Theme>(Cache, CoreCacheKeys.ThemeKeys.ThemeByType(themeType))
                .Return<Theme>(() => _themeRepository.Table.SingleOrDefault(t => t.ThemeType == themeType.ToString()));

            return theme.ThemeName;
        }

        public void SetTheme(string themeName, ThemeType themeType = ThemeType.Portal)
        {
            Theme theme = _themeRepository.Table.SingleOrDefault(t => t.ThemeType == themeType.ToString());
            theme.ThemeName = themeName;
            theme.ThemeType = themeType.ToString();
            _themeRepository.SubmitChanges();

            Cache.Remove(CoreCacheKeys.ThemeKeys.ThemeByType(themeType));
        }

        public IEnumerable<Theme> GetAllThemes()
        {
            return _themeRepository.Table.ToList();
        }
    }
}
