using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Mvc.Controllers;

namespace Falcon.Modules.Contents.Controllers
{
    public class AppsController : BaseController
    {
        //
        // GET: /Apps/
        [Layout("App")]
        public ActionResult Index()
        {
            return View();
        }
	}
}