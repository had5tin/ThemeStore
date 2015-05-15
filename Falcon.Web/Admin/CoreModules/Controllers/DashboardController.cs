using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Falcon.Mvc.Controllers;
using Falcon.Infrastructure;

namespace Falcon.Admin.CoreModules.Controllers
{
    public class DashboardController : AdminBaseController
    {
        public DashboardController()
        {            
        }

        //
        // GET: /Admin/ or /Admin/Dashboard
        public ActionResult Index()
        {                        
            return View();
        }
    }
}
