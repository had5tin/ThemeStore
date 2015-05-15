using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Falcon.Mvc.Controllers;
using Falcon.Security;

using Falcon.Services.Users;
using Falcon.Data.Domain;
using Falcon.Admin.CoreModules.Models;

namespace Falcon.Admin.CoreModules.Controllers
{    
    public class AuthorizationController : AdminBaseController
    {
        public AuthorizationController()
        {            
        }
        //
        // GET: /Admin/Authorization/

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Roles");
        }             
    }
}
