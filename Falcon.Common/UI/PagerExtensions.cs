using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Falcon.Common.UI
{
    public static class PagerExtensions
    {
        public static Pager Pager(this HtmlHelper helper, PaginationModels pagination)
        {
            return new Pager(pagination, helper.ViewContext.HttpContext.Request);
        }
    }
}
