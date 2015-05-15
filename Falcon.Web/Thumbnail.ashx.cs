using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Falcon.Infrastructure;
using Falcon.Services.Thumbnails;
using Falcon.Common;
using System.IO;

namespace Falcon.Web
{
    /// <summary>
    /// Tạo Thumbnail cho ảnh
    /// </summary>
    public class Thumbnail : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string image = context.Request.QueryString["i"];
            string size = context.Request.QueryString["s"];
            var cache = String.Equals(context.Request.QueryString["c"], "1");

            if (!string.IsNullOrEmpty(image) && image.StartsWith("http") == false)
            {
                ThumbSizeEnum thumbSize = GetThumbSize(size);
                string subPath;
                if (image.StartsWith("~/"))
                {
                    subPath = image.Substring(1);
                }
                else
                {
                    subPath = image;
                }

                string currentPath = "~/Thumbnail/" + thumbSize.ToString() + subPath;
                try
                {
                    currentPath = context.Server.MapPath(currentPath);

                    if (File.Exists(context.Server.MapPath(image)))
                    {
                        if (cache && File.Exists(currentPath))
                        {
                            OutputCacheResponse(context, File.GetLastWriteTime(currentPath));
                            context.Response.ContentType = "image/jpg";
                            context.Response.WriteFile(currentPath);
                        }
                        else
                        {
                            var thumbnail = new Falcon.Services.Thumbnails.Thumbnail(EngineContext.Current.Resolve<IThumbnailSettingService>());

                            bool saveSuccess = thumbnail.CreateThumnail(thumbSize, image);
                            // Tạo luôn tat cả kích cỡ 
                            thumbnail.CreateAllThumbnails(image);

                            if (saveSuccess && File.Exists(currentPath))
                            {
                                context.Response.ContentType = "image/jpg";
                                context.Response.WriteFile(currentPath);
                            }
                            else
                            {
                                ReturnDefaultImage(context);
                            }
                        }
                    }
                    else
                    {
                        ReturnDefaultImage(context);
                    }
                }
                catch
                {
                    ReturnDefaultImage(context);
                }
            }
            else
            {
                ReturnDefaultImage(context);
            }
        }

        private void ReturnDefaultImage(HttpContext context)
        {
            string size = context.Request.QueryString["s"];
            if (!String.IsNullOrEmpty(size))
            {
                ThumbSizeEnum thumbSize = GetThumbSize(size);
                if (thumbSize == ThumbSizeEnum.Small)
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(context.Server.MapPath("~/Content/logo_64_64.png"));
                }
                else if (thumbSize == ThumbSizeEnum.Medium)
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(context.Server.MapPath("~/Content/logo_64_64.png"));
                }
                else if (thumbSize == ThumbSizeEnum.Large)
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(context.Server.MapPath("~/Content/logo_64_64.png"));
                }
                else if (thumbSize == ThumbSizeEnum.ExtraLarge)
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(context.Server.MapPath("~/Content/logo_340_300.png"));
                }
                else if (thumbSize == ThumbSizeEnum.Pinterest)
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(context.Server.MapPath("~/Content/logo_214.png"));
                }
            }
            else
            {
                context.Response.ContentType = "image/png";
                context.Response.WriteFile(context.Server.MapPath("~/Content/logo_64_64.png"));
            }
        }

        private ThumbSizeEnum GetThumbSize(string s)
        {
            switch (s)
            {
                case "s": return ThumbSizeEnum.Small;
                case "m": return ThumbSizeEnum.Medium;
                case "l": return ThumbSizeEnum.Large;
                case "xl": return ThumbSizeEnum.ExtraLarge;
                case "p": return ThumbSizeEnum.Pinterest;
                default: return ThumbSizeEnum.Small;
            }
        }

        private static void OutputCacheResponse(HttpContext context, DateTime lastModified)
        {
            var cachePolicy = context.Response.Cache;
            cachePolicy.SetCacheability(HttpCacheability.Public);
            cachePolicy.VaryByParams["s"] = true;
            cachePolicy.VaryByParams["i"] = true;
            //cachePolicy.VaryByParams["nocache"] = true;
            cachePolicy.SetOmitVaryStar(true);
            cachePolicy.SetExpires(DateTime.Now + TimeSpan.FromDays(365));
            cachePolicy.SetValidUntilExpires(true);
            cachePolicy.SetLastModified(lastModified);
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}