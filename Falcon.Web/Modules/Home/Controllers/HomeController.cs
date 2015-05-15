using Falcon.Mvc.Controllers;
using Falcon.Services.Collections;
using Falcon.Services.Features;
using System.Web.Mvc;
using Falcon.Services.ThemeStores;

namespace Falcon.Modules.Home.Controllers
{
    public class HomeController : BaseController
    {

        private readonly IFeatureService _featureService;
        private readonly ICollectionService _collectionService;
        private readonly IThemeStoreService _themeStoreService;

        public HomeController(IFeatureService featureService, ICollectionService collectionService, IThemeStoreService themeStoreService)
        {
            _featureService = featureService;
            _collectionService = collectionService;
            _themeStoreService = themeStoreService;
        }

        //
        // GET: /Home/Home/       
        [Layout("home")]
        public ActionResult Index()
        {
            var model = _themeStoreService.GetAll();
            return View(model);
        }

        public PartialViewResult Feature()
        {
            var model = _featureService.GetAllItems();
            return PartialView("_Feature", model);
        }
        public PartialViewResult Collection()
        {
            var model = _collectionService.GetAllItems();
            return PartialView("_Collection", model);
        }

        //public PartialViewResult ThemeFeature()
        //{
        //    var model = _themeFeatureService.GetThemeFeatureByFeatureId(3);
        //    return PartialView("_ThemeFeature", model);
        //}
    }
}
