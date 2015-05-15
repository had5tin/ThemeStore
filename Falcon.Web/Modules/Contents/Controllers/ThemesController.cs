
using Falcon.Data.Domain;
using Falcon.Mvc.Controllers;
using System.Web.Mvc;
using Falcon.Services.ThemeStores;

namespace Falcon.Modules.Contents.Controllers
{
    public class ThemesController : BaseController
    {
        private readonly IThemeStoreService _themeStoreService;

        public ThemesController(IThemeStoreService themeStoreService)
        {
            _themeStoreService = themeStoreService;
        }

        //

        [Layout("Default")]
        public ActionResult Details(string title)
        {
            ThemeStore model = _themeStoreService.GetThemeBySeoUrl(title);
            if (model == null)
            {
                return ShowErrorPage("Xin lỗi bạn, hệ thống không tìm thấy trang: " + title);
            }
            ViewBag.Title = model.Name;
            return View(model);
        }

        [Layout("Home")]
        public ActionResult Search(string search)
        {
            search += "";
            if (search == "")
            {
                return Redirect("/");
            }
            var model = _themeStoreService.GetThemeByName(search);
            ViewBag.Search = search;
            return View(model);
        }
        [Layout("Home")]
        public ActionResult Query()
        {
            var model = _themeStoreService.GetAll();

            var arrStr = new string[4];
            arrStr[0] = Request["price"];
            arrStr[1] = Request["feature"];
            arrStr[2] = Request["collection"];
            arrStr[3] = Request["sort"];

            model = _themeStoreService.GetThemeByQuery(arrStr);
            return PartialView(model);
        }
        public ActionResult Error(string path)
        {
            return View();
            //return ShowErrorPage("Xin lỗi bạn, hệ thống không tìm thấy trang: " + path);
        }
    }
}
