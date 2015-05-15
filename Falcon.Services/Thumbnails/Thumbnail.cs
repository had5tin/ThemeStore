using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Falcon.Data.Domain;
using System.Web;
using Falcon.Common;
//using Microsoft.Test.Tools.WicCop.InteropServices.ComTypes;
//using WicResize.InteropServices;
using ImageResizer;

namespace Falcon.Services.Thumbnails
{
    public class Thumbnail
    {
        private readonly IThumbnailSettingService _thumbService;

        public Thumbnail(IThumbnailSettingService thumbService)
        {
            this._thumbService = thumbService;
        }

        public void CreateAllThumbnails(string imagePath)
        {
            CreateThumnail(ThumbSizeEnum.Small, imagePath);
            CreateThumnail(ThumbSizeEnum.Medium, imagePath);
            CreateThumnail(ThumbSizeEnum.Large, imagePath);
            CreateThumnail(ThumbSizeEnum.ExtraLarge, imagePath);
        }

        public bool CreateThumnail(ThumbSizeEnum thumbSize, string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    return false;
                }
                if (File.Exists(HttpContext.Current.Server.MapPath(imagePath)))
                {
                    string subPath;
                    if (!imagePath.StartsWith("~/"))
                    {
                        imagePath = "~" + imagePath;
                        
                    }
                    subPath = imagePath.Substring(1, imagePath.LastIndexOf("/"));

                    string savePath = "~/Thumbnail/" + thumbSize.ToString() + subPath;

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(savePath)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(savePath));
                    }

                    savePath += imagePath.Substring(imagePath.LastIndexOf("/"));

                    ThumbnailSetting setting = _thumbService.GetByThumbSize(thumbSize);
                    if (setting == null)
                    {
                        return false;
                    }

                    return CreateImage(imagePath, setting.Width, setting.Height, savePath);
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CreateCropThumbnail(int width, int height, string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    return false;
                }
                if (File.Exists(HttpContext.Current.Server.MapPath(imagePath)))
                {
                    string subPath;
                    if (!imagePath.StartsWith("~/"))
                    {
                        imagePath = "~" + imagePath;

                    }
                    subPath = imagePath.Substring(1, imagePath.LastIndexOf("/"));

                    string savePath = "~/Thumbnail/Cache/" + subPath;

                    if (!Directory.Exists(HttpContext.Current.Server.MapPath(savePath)))
                    {
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath(savePath));
                    }

                    savePath += imagePath.Substring(imagePath.LastIndexOf("/"));
                    int index = savePath.LastIndexOf(".");
                    savePath = savePath.Substring(0, index) + "_" + width + "_" + height + savePath.Substring(index);
                    if (width == 0 || height == 0)
                    {
                        return false;
                    }

                    return CreateImageCrop(imagePath, width, height, savePath);
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static void DeleteImageAndThumbs(string imagePath)
        {
            string realPath = HttpContext.Current.Server.MapPath(imagePath);
            if (File.Exists(realPath))
            {
                File.Delete(realPath);
            }

            if (imagePath.StartsWith("~/"))
            {
                imagePath = imagePath.Substring(1);
            }

            string smallRealPath = HttpContext.Current.Server.MapPath("~/Thumbnail/" + ThumbSizeEnum.Small.ToString() + imagePath);
            if (File.Exists(smallRealPath))
            {
                File.Delete(smallRealPath);
            }

            string mediumRealPath = HttpContext.Current.Server.MapPath("~/Thumbnail/" + ThumbSizeEnum.Medium.ToString() + imagePath);
            if (File.Exists(mediumRealPath))
            {
                File.Delete(mediumRealPath);
            }

            string largeRealPath = HttpContext.Current.Server.MapPath("~/Thumbnail/" + ThumbSizeEnum.Large.ToString() + imagePath);
            if (File.Exists(largeRealPath))
            {
                File.Delete(largeRealPath);
            }

            string extraLargeRealPath = HttpContext.Current.Server.MapPath("~/Thumbnail/" + ThumbSizeEnum.ExtraLarge.ToString() + imagePath);
            if (File.Exists(extraLargeRealPath))
            {
                File.Delete(extraLargeRealPath);
            }
        }

        public static void RenameImage(string imagePath, string newimagePath)
        {
            string realPath = HttpContext.Current.Server.MapPath(imagePath);
            if (File.Exists(realPath))
            {
                File.Move(realPath, HttpContext.Current.Server.MapPath(newimagePath));                
            }
        }

        private bool CreateImageCrop(string imagePath, int imageWidth, int imageHeight, string savePath)
        {
            if (imageWidth < 0 || imageHeight < 0 || (imageWidth == 0 && imageHeight == 0))
            {
                return false;
            }

            ImageResizer.ImageJob i = new ImageResizer.ImageJob(imagePath, savePath, new ImageResizer.Instructions(
              "width=" + imageWidth + "&height=" + imageHeight + "&mode=crop&scache=disk&format=jpg"));
            i.CreateParentDirectory = true; //Auto-create the uploads directory.
            i.Build();

            return true;
        }

        private bool CreateImage(string imagePath, int imageWidth, int imageHeight, string savePath)
        {
            //Trường hợp 1 trong 2 giá trị âm hoặc cùng = 0 thì ko tạo ảnh thumb
            if (imageWidth < 0 || imageHeight < 0 || (imageWidth == 0 && imageHeight == 0))
            {
                return false;
            }
            //Duong dan thuc cua file goc
            //imagePath = HttpContext.Current.Server.MapPath(imagePath);
            //Duong dan thuc cua file thumbnail
            //savePath = HttpContext.Current.Server.MapPath(savePath);
            //Tao file thumbnail

            //ImageBuilder.Current.Build(imagePath, savePath, new ResizeSettings("width=" + imageWidth + "&height=" + imageHeight + "&mode=max&format=jpg"), false, true);
            
            ImageResizer.ImageJob i = new ImageResizer.ImageJob(imagePath, savePath, new ImageResizer.Instructions(
              "width=" + imageWidth + "&height=" + imageHeight + "&mode=max&format=jpg"));
            i.CreateParentDirectory = true; //Auto-create the uploads directory.
            i.Build();

            return true;

            //// Read the source image
            //var photo = File.ReadAllBytes(imagePath);
            //var factory = (IWICComponentFactory)new WICImagingFactory();
            //var inputStream = factory.CreateStream();
            //inputStream.InitializeFromMemory(photo, (uint)photo.Length);
            //var decoder = factory.CreateDecoderFromStream(inputStream, null, WICDecodeOptions.WICDecodeMetadataCacheOnLoad);
            //var frame = decoder.GetFrame(0);
            //// Compute target size
            //uint width, height, thumbWidth, thumbHeight;
            //frame.GetSize(out width, out height);

            ////if (width > height)
            ////{
            ////    thumbWidth = (uint)imageWidth;
            ////    thumbHeight = (uint)(height * imageHeight / imageWidth);
            ////}
            ////else
            ////{
            ////    thumbWidth = (uint)(width * imageWidth / height);
            ////    thumbHeight = (uint)imageHeight;
            ////}
            
            //if (imageHeight == 0 || imageHeight * width > imageWidth * height)
            //{
            //    thumbHeight = (uint)((imageWidth * height) / width);
            //    thumbWidth = (uint)imageWidth;
            //}
            //else
            //{
            //    thumbWidth = (uint)((imageHeight * width) / height);
            //    thumbHeight = (uint)imageHeight;
            //}
            //using (MemoryIStream outputStream = new MemoryIStream())
            //{
            //    // Prepare JPG encoder
            //    var encoder = factory.CreateEncoder(Consts.GUID_ContainerFormatJpeg, null);
            //    encoder.Initialize(outputStream, WICBitmapEncoderCacheOption.WICBitmapEncoderNoCache);
            //    // Prepare output frame
            //    IWICBitmapFrameEncode outputFrame;
            //    var arg = new IPropertyBag2[1];
            //    encoder.CreateNewFrame(out outputFrame, arg);
            //    var propBag = arg[0];
            //    var propertyBagOption = new PROPBAG2[1];
            //    propertyBagOption[0].pstrName = "ImageQuality";
            //    propBag.Write(1, propertyBagOption, new object[] { 0.85F });
            //    outputFrame.Initialize(propBag);
            //    outputFrame.SetResolution(96, 96);
            //    outputFrame.SetSize(thumbWidth, thumbHeight);
            //    // Prepare scaler
            //    var scaler = factory.CreateBitmapScaler();
            //    scaler.Initialize(frame, thumbWidth, thumbHeight, WICBitmapInterpolationMode.WICBitmapInterpolationModeFant);
            //    // Write the scaled source to the output frame
            //    outputFrame.WriteSource(scaler, new WICRect { X = 0, Y = 0, Width = (int)thumbWidth, Height = (int)thumbHeight });
            //    outputFrame.Commit();
            //    encoder.Commit();
            //    var outputArray = outputStream.ToArray();
            //    // Write to the cache file
            //    File.WriteAllBytes(savePath, outputArray);
            //}            
        }
    }
}
