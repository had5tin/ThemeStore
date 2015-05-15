using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace Falcon.Common.UI
{
    public static class FormatDateExtensions
    {
        public static MvcHtmlString FormatDate(this HtmlHelper helper, DateTime? inputDate, string dateFormat = "dd/MM/yyyy")
        {
            if (inputDate != null)
            {
                DateTime date = (DateTime)inputDate;
                CultureInfo ci = CultureInfo.InvariantCulture;
                return MvcHtmlString.Create(date.ToString(dateFormat, ci));
            }
            else
            {
                return MvcHtmlString.Create("");
            }
            
        }

        public static MvcHtmlString FormatDateTime(this HtmlHelper helper, DateTime? inputDate, string dateFormat = "dd/MM/yyyy HH:mm:ss")
        {
            return FormatDate(helper, inputDate, dateFormat);
        }

        public static MvcHtmlString FormatDateTimeDiff(this HtmlHelper helper, DateTime? date, string format = "{0:HH:mm | dd/MM/yyyy}", string seperate = "|")
        {
            if (date != null)
            {
                return MvcHtmlString.Create(Util.GetTimeDiff((DateTime)date, format, seperate));
            }
            else
            {
                return MvcHtmlString.Create("");
            }
        }
    }
}
