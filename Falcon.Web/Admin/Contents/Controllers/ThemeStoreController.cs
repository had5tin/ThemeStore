using System.Linq;
using System.Web.Mvc;
using Falcon.Admin.Modules.Contents.Models;
using Falcon.Data.Domain;
using Falcon.Mvc.Controllers;
using Falcon.Services.Collections;
using Falcon.Services.Features;
using Falcon.Services.ThemeStores;

namespace Falcon.Admin.Modules.Contents.Controllers
{
    public class ThemeStoreController : AdminBaseController
    {
        private readonly IThemeStoreService _themeStoreService;
        private readonly ICollectionService _collectionService;
        public ThemeStoreController(IThemeStoreService themeStoreService, ICollectionService collectionService)
        {
            _themeStoreService = themeStoreService;
            _collectionService = collectionService;
        }


        //
        // GET: /Colletion/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            var model = _themeStoreService.GetAll();
            return PartialView("_List", model);
        }
        //public ActionResult Add()
        //{
        //    return PartialView("_Add");
        //}

        //[HttpPost]
        //public ActionResult Add(Collection model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _themeUService.Add(model);
        //        string url = Url.Action("List", "Collection");
        //        return Json(new { success = true, url = url });
        //    }
        //    return PartialView("_Add", model);
        //}
        //public ActionResult Delete(int Id)
        //{
        //    return PartialView("_Delete", _collectionService.GetById(Id));
        //}
        //[HttpPost]
        //public ActionResult Delete(Collection model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _themeUService.Del(model.Id);
        //        string url = Url.Action("List", "Collection");
        //        return Json(new { success = true, url = url });
        //    }
        //    return PartialView("_Delete", model);
        //}
        public ActionResult ThemeCollection(int Id)
        {
            var model = new ThemeCollectionModel()
            {
                Id = Id,
                CollectionSelect = _collectionService.GetByThemeId(Id),
                CollectionList = _collectionService.GetAllItems().ToList(),
            };
            return PartialView("_ThemeCollection", model);
        }
    }
}