using System.Web.Mvc;
using Falcon.Mvc;

namespace Falcon.Admin.Modules.Contents
{
    public class ContentsAreaRegistration : AreaRegistration
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
                "pages_default",
                "admin/pages/{action}/{id}",
                new { controller = "Pages", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "feature_default",
                "admin/feature/{action}/{id}",
                new { controller = "Feature", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "collection_default",
                "admin/collection/{action}/{id}",
                new { controller = "Collection", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                "themeu_default",
                "admin/ThemeStore/{action}/{id}",
                new { controller = "ThemeStore", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
