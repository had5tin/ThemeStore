using Falcon.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Falcon.Common.UI
{
    public static class ImageExtensions
    {
        public static MvcHtmlString ImageActionLink(this HtmlHelper htmlHelper, ThumbSizeEnum thumbSize, string imgSrc, string alt, object htmlAttributes = null, object imgHtmlAttributes = null)
        {
            string path = "";
            if (imgSrc != null)
            {
                if (imgSrc.StartsWith("~/"))
                    path = imgSrc.Substring(1);
                else
                    path = imgSrc;
            }
            imgSrc = ThumbnailExtensions.Thumbnail(htmlHelper, thumbSize, imgSrc, alt, alt, imgHtmlAttributes, true, false).ToString();
            string url = path;

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgSrc;
            // imglink.MergeAttributes((IDictionary<string, string>)htmlAttributes, true);
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(imglink.ToString());
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null, object imgHtmlAttributes = null)
        {
            string path = "";
            if (imgSrc != null)
            {
                if (imgSrc.StartsWith("~/"))
                    path = imgSrc.Substring(1);
                else
                    path = imgSrc;
                path = EngineContext.Current.FalconConfig.MediaDomainName + path;
            }
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", path);

            if (alt.Length > 70)
            {
                alt = Util.SubString(alt, 70, false);
            }

            imgTag.MergeAttribute("alt", alt);
            imgTag.MergeAttribute("title", alt);
            imgTag.MergeAttributes(new RouteValueDictionary(imgHtmlAttributes));
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgTag.ToString(TagRenderMode.SelfClosing);
            // imglink.MergeAttributes((IDictionary<string, string>)htmlAttributes, true);
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(imglink.ToString());
        }



        /// <summary>
        /// Tạo Link Image với ảnh thumb
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="thumbSize"></param>
        /// <param name="imgSrc"></param>
        /// <param name="alt"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="imgHtmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageActionLink(this HtmlHelper htmlHelper, ThumbSizeEnum thumbSize, string imgSrc, string alt, string actionName, string controllerName, object routeValues = null, object htmlAttributes = null, object imgHtmlAttributes = null, bool lazyload = false)
        {
            object imgAtt;
            if (imgHtmlAttributes == null)
            {
                imgAtt = new { alt = alt };
            }
            else
            {
                imgAtt = imgHtmlAttributes;
            }
            imgSrc = ThumbnailExtensions.Thumbnail(htmlHelper, thumbSize, imgSrc, alt, alt, imgAtt, true, lazyload).ToString();
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            //TagBuilder imgTag = new TagBuilder("img");
            //imgTag.MergeAttribute("src", imgSrc);
            //imgTag.MergeAttributes((IDictionary<string, string>)imgHtmlAttributes, true);
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgSrc;
            //imglink.MergeAttributes((IDictionary<string, string>)htmlAttributes, true);
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return MvcHtmlString.Create(imglink.ToString());
        }

        public static MvcHtmlString ImageActionLinkPromotion(this HtmlHelper htmlHelper, ThumbSizeEnum thumbSize, string imgSrc, string alt, string actionName, string controllerName, int PromotionRate, object routeValues = null, object htmlAttributes = null, object imgHtmlAttributes = null)
        {
            imgSrc = ThumbnailExtensions.Thumbnail(htmlHelper, thumbSize, imgSrc, alt, alt, imgHtmlAttributes).ToString();
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            string url = urlHelper.Action(actionName, controllerName, routeValues);

            TagBuilder tagDivRoot = new TagBuilder("div");
            tagDivRoot.MergeAttribute("class", "image_sale");
            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            //imglink.InnerHtml = imgSrc.ToString();
            TagBuilder tagDiv = new TagBuilder("div");
            if (PromotionRate >= 40)
            {
                tagDiv.MergeAttribute("class", "sale_off_hot");
            }
            else
            {
                tagDiv.MergeAttribute("class", "sale_off");
            }
            tagDiv.InnerHtml = "-" + PromotionRate.ToString() + "%";
            imglink.InnerHtml = imgSrc.ToString();
            imglink.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagDivRoot.InnerHtml += imglink.ToString();
            tagDivRoot.InnerHtml += tagDiv.ToString();
            return MvcHtmlString.Create(tagDivRoot.ToString());
        }
    }
}
