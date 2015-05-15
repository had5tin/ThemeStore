using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Falcon.Mvc.Controllers;
using Falcon.Security;

using Falcon.Data.Domain;
using Falcon.Admin.CoreModules.Models;
using Falcon.Themes;

namespace Falcon.Admin.CoreModules.Controllers
{    
    public class ThemeController : AdminBaseController
    {
        private readonly IThemeService _themeService;
        private readonly IThemeProvider _themeProvider;

        public ThemeController(IThemeService themeService, IThemeProvider themeProvider)
        {
            this._themeService = themeService;
            this._themeProvider = themeProvider;
        }

        //
        // GET: /Admin/Themes/

        public ActionResult Index()
        {
            IEnumerable<Theme> themes = _themeService.GetAllThemes();
            List<ThemeModel> models = new List<ThemeModel>();
            ThemeConfiguration themeConfig;
            foreach (Theme theme in themes)
            {
                themeConfig = _themeProvider.GetThemeConfiguration(theme.ThemeName);
                if (themeConfig != null)
                {
                    models.Add(new ThemeModel()
                    {
                        VirtualPath = themeConfig.VirtualPath,                        
                        ThemeName = theme.ThemeName,
                        ThemeType = theme.ThemeType,
                        ThemeTitle = themeConfig.ThemeTitle,
                        Modified = theme.Modified
                    });
                }
            }
            return View(models);
        }

        public ActionResult Reload()
        {
            _themeProvider.ReloadConfigurations();
            return Content("Reload Themes Success");
        }
    }
}
