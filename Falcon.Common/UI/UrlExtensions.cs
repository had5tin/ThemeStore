using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Falcon.Infrastructure;

namespace Falcon.Common.UI
{
    public static class UrlExtensions
    {
        /// <summary>
        /// Tạo đường link sử dụng Media Domain (sử dụng cho image, thumbnail, flash...)
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="path"></param>
        /// <param name="includeMediaDomain"></param>
        /// <returns></returns>
        public static string Content(this UrlHelper urlHelper, string path, bool includeMediaDomain = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "";
            }
            if (includeMediaDomain)
            {
                return EngineContext.Current.FalconConfig.MediaDomainName + urlHelper.Content(path);
            }
            else
            {
                return urlHelper.Content(path);
            }
        }

        /// <summary>
        /// Tạo đường link sử dụng Static Domain (sử dụng cho link đến file .js, .css, .ico)
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="path"></param>
        /// <param name="includeStaticDomain"></param>
        /// <returns></returns>
        public static string ContentStatic(this UrlHelper urlHelper, string path, bool includeStaticDomain = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return "";
            }
            if (includeStaticDomain)
            {
                return EngineContext.Current.FalconConfig.StaticDomainName + urlHelper.Content(path);
            }
            else
            {
                return urlHelper.Content(path);
            }
        }
    }
}
