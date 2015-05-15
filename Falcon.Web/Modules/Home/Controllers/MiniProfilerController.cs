using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace Falcon.Modules.Home.Controllers
{
    public class MiniProfilerController : Controller
    {
        //
        // GET: /MiniProfiler/

        public ActionResult Set(string id)
        {
            string validKey = ConfigurationManager.AppSettings["MiniProfilerKey"];
            if (validKey.Equals(id))
            {
                var cookie = new HttpCookie("MiniProfilerKey", validKey);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);
                return Content("Success");
            }
            else
            {
                return Content("Invalid key");
            }

        }

        public ActionResult UnSet(string id)
        {
            string validKey = ConfigurationManager.AppSettings["MiniProfilerKey"];
            if (validKey.Equals(id))
            {
                HttpCookie cookie = new HttpCookie("MiniProfilerKey");
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
            return Content("Success");
        }
    }
}
