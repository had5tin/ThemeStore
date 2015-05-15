using System.Web.Mvc;
using Falcon.Mvc.Controllers;
using Falcon.Services.Features;

namespace Falcon.Admin.Modules.Contents.Controllers
{
    public class FeatureController : AdminBaseController
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        //
        // GET: /Feature/
        public ActionResult Index()
        {
            var model = _featureService.GetAll();
            return View(model);
        }
	}
}