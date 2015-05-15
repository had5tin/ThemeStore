using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace Falcon.Common
{
    public class CaptchaImageResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {            
            Bitmap bmp = new Bitmap(70, 25);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.WhiteSmoke);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            string refresh = context.RequestContext.HttpContext.Request.QueryString["r"];

            string randomString;
            if ("0".Equals(refresh))
            {
                if (context.HttpContext.Session["captchastring"] == null)
                {
                    randomString = Util.RandomCaptcha(4);
                    context.HttpContext.Session["captchastring"] = randomString;
                }
                else
                {
                    randomString = context.HttpContext.Session["captchastring"].ToString();                
                }
            }
            else
            {
                randomString = Util.RandomCaptcha(4);
                context.HttpContext.Session["captchastring"] = randomString;
            }
                        
            Font drawFont = new Font("Arial", 15, FontStyle.Bold);            
            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(51,51,51));
            // Create point for upper-left corner of drawing.
            float x = 2.0F;
            float y = 2.0F;
            // Set format of string.
            StringFormat drawFormat = new StringFormat();            
            //drawFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft;
            // Draw string to screen.
            g.DrawString(randomString, drawFont, drawBrush, x, y, drawFormat);

           // g.DrawString(randomString, new Font("Courier", 12), new SolidBrush(Color.WhiteSmoke), 2, 2);
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "image/png";
            bmp.Save(response.OutputStream, ImageFormat.Png);
            
            //clean
            bmp.Dispose();
            g.Dispose();
            drawBrush.Dispose();
            drawFont.Dispose();
        }
    }
}