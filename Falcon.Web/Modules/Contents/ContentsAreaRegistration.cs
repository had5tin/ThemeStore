using System.Web.Mvc;
using Falcon.Mvc;

namespace Falcon.Modules.Contents
{
    public class PagesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contents";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Pages_Details",
                "pages/{title}.html",
                new { controller = "Pages", action = "Details" },
                new { title = new NotEqualConstraint("Error") }
            );
            context.MapRoute(
                 "Pages_Error",
                 "pages/error",
                 new { controller = "Pages", action = "Error" }
             );
            context.MapRoute(
                "Giaodien_Details",
                "ThemeStore/Details/{title}",
                new { controller = "Themes", action = "Details" },
                new { title = new NotEqualConstraint("Error") }
            );
            context.MapRoute(
                "SearchThemes_default",
                "ThemeStore/{action}/{id}",
                new { controller = "Themes", action = "Query", id = UrlParameter.Optional }
            );
        }
    }
}
