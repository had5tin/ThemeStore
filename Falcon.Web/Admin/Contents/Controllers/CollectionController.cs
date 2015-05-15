using System.Web.Mvc;
using Falcon.Data.Domain;
using Falcon.Mvc.Controllers;
using Falcon.Services.Collections;
using Falcon.Services.Features;

namespace Falcon.Admin.Modules.Contents.Controllers
{
    public class CollectionController : AdminBaseController
    {
        private readonly ICollectionService _collectionService;

        public CollectionController(ICollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        //
        // GET: /Colletion/
        public ActionResult Index()
        {
            //var model = _collectionService.GetAll();
            return View();
        }

        public ActionResult List()
        {
            var model = _collectionService.GetAll();
            return PartialView("_List", model);
        }
        public ActionResult Add()
        {
            return PartialView("_Add");
        }

        [HttpPost]
        public ActionResult Add(Collection model)
        {
            if (ModelState.IsValid)
            {
                _collectionService.Add(model);
                string url = Url.Action("List", "Collection");
                return Json(new { success = true, url = url });
            }
            return PartialView("_Add", model);
        }
        public ActionResult Delete(int Id)
        {
            return PartialView("_Delete", _collectionService.GetById(Id));
        }
        [HttpPost]
        public ActionResult Delete(Collection model)
        {
            if (ModelState.IsValid)
            {
                _collectionService.Del(model.Id);
                string url = Url.Action("List", "Collection");
                return Json(new { success = true, url = url });
            }
            return PartialView("_Delete", model);
        }
        public ActionResult Edit(int Id)
        {
            return PartialView("_Edit", _collectionService.GetById(Id));
        }
        [HttpPost]
        public ActionResult Edit(Collection model)
        {
            if (ModelState.IsValid)
            {
                _collectionService.Edit(model);
                string url = Url.Action("List", "Collection");
                return Json(new { success = true, url = url });
            }
            return PartialView("_Edit", model);
        }
    }
}