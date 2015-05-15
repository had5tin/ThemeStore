using System.Web.Mvc;
using Falcon.Mvc;

namespace Falcon.Admin.CoreModules
{
    public class UsersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "CoreModules";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Authorization_default",
                "admin/authorization/{action}/{id}",
                new { controller = "Authorization", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Dashboard_default",
                "admin/dashboard/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "elmahLog_default",
                "admin/elmahLog/{action}/{id}",
                new { controller = "ElmahLog", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Licence_default",
                "admin/licence/{action}/{id}",
                new { controller = "Licence", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "login_default",
                "admin/login/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "Roles_default",
                "admin/roles/{action}/{id}",
                new { controller = "Roles", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "systemsetting_default",
                "admin/systemsetting/{action}/{id}",
                new { controller = "SystemSetting", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "theme_default",
                "admin/theme/{action}/{id}",
                new { controller = "Theme", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "user_default",
                "admin/user/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
