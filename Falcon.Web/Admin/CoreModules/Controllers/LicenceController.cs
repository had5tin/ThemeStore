using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Falcon.Admin.CoreModules.Controllers
{
    public class LicenceController : Controller
    {
        //
        // GET: /Licence/Details/5

        public ActionResult Invalid()
        {
            return Content("Licence Invalid. Contact us at khoinm@dkt.com.vn.");
        }
    }
}
