using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Falcon.Infrastructure;
using Falcon.Common;

namespace Falcon.Common.UI
{
    public static class ThumbnailExtensions
    {
        public static MvcHtmlString Thumbnail(this HtmlHelper helper, ThumbSizeEnum thumbSize, string imagePath, object htmlAttributes = null, bool includeMediaDomain = true, bool lazyload = false, bool cache = false)
        {
            return Thumbnail(helper, thumbSize, imagePath, "", "", htmlAttributes, includeMediaDomain, lazyload, cache);
        }

        public static MvcHtmlString Thumbnail(this HtmlHelper helper, ThumbSizeEnum thumbSize, string imagePath, string imageAlt = "", string imageTitle = "", object htmlAttributes = null, bool includeMediaDomain = true, bool lazyload = false, bool cache = false)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return MvcHtmlString.Create(string.Empty);
            }

            string currentPath = Util.GetThumbLink(imagePath, thumbSize, includeMediaDomain);

            var img = new TagBuilder("img");
            var noscript = new TagBuilder("noscript");

            if (lazyload == true)
            {
                img.MergeAttribute("class", "lazyload");
                img.MergeAttribute("src", "/Themes/Portal/Default/Images/bg_lazyload_140.gif");
                img.MergeAttribute("data-original", currentPath);

                noscript.InnerHtml = "<img src='" + currentPath + "'/>";
            }
            else
            {
                img.MergeAttribute("src", currentPath);
            }

            if (!string.IsNullOrEmpty(imageAlt))
            {
                img.MergeAttribute("alt", imageAlt);
            }

            if (!string.IsNullOrEmpty(imageTitle))
            {
                img.MergeAttribute("title", imageTitle);
            }
            
            img.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            var c = cache ? 1 : 0;
            var onerror = string.Format("/Thumbnail.ashx?i={0}&s={1}&c={2}", imagePath, GetImageSize(thumbSize), c);           
            if (lazyload)
            {
                img.MergeAttribute("data-onerror", onerror);
            }
            else
            {
                img.MergeAttribute("onerror", "this.src='" + onerror + "'");
            }
            
            if (lazyload)
            {
                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing) + noscript.ToString());
            }
            else
            {
                return MvcHtmlString.Create(img.ToString(TagRenderMode.SelfClosing));
            }
            
        }

        public static MvcHtmlString ThumbnailUrl(this HtmlHelper helper, string imagePath, ThumbSizeEnum thumbSize, bool includeMediaDomain = true)
        {
            return MvcHtmlString.Create(Util.GetThumbLink(imagePath, thumbSize, includeMediaDomain));
        }

        public static string GetImageSize(ThumbSizeEnum size)
        {
            switch (size)
            {
                case ThumbSizeEnum.Small: return "s";
                case ThumbSizeEnum.Medium: return "m";
                case ThumbSizeEnum.Large: return "l";
                case ThumbSizeEnum.ExtraLarge: return "xl";
                default: return "s";
            }
        }
               
    }
}
