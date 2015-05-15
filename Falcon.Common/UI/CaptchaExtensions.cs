using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.Common.UI
{
    public static class CaptchaExtensions
    {
        /// <summary>
        /// Dùng trong trường hợp trên trang chỉ có nhiều ô nhập captcha
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="refresh">Có refresh lại mã captcha không. Nên dùng refresh = false trong trường hợp hiển thị nhiều mã captcha trên một trang.</param>
        /// <returns></returns>
        public static MvcHtmlString Captcha(this HtmlHelper helper, string id, bool refresh = true, bool autoRefresh = true)
        {
            Random rand = new Random();
            string a = rand.NextDouble().ToString();
            if (string.IsNullOrEmpty(id))
            {
                id = "Captcha";
            }
            int r = 0;
            if (refresh)
            {
                r = 1;
            }

            if (autoRefresh)
            {
                return new MvcHtmlString(string.Format(@"<table><tr><td><input type='text' value='' style='width:100px;height:20px;border:1px solid #DFDFDF;vertical-align:middle;' name='{0}' id='{0}' data-val-required='Nhập vào mã an toàn' data-val-length-min='4' data-val-length-max='20' data-val-length='Bạn đã nhập sai mã an toàn.' data-val='true' class='input-text' onfocus=""$('#img_{0}').attr('src', '/Ajax/Capcha/ShowCaptchaImage?r={2}&a=' + Math.random())""></td>
<td><img alt='Mã captcha' src='/Ajax/Capcha/ShowCaptchaImage?r={2}&a={1}' id='img_{0}' style='vertical-align:middle;'></td></tr><tr><td colspan='2'><span data-valmsg-replace='true' data-valmsg-for='{0}' class='field-validation-valid'></span></td></tr></table>", id, a, r));
            }
            else
            {
                return new MvcHtmlString(string.Format(@"<table><tr><td><input type='text' value='' style='width:100px;height:20px;border:1px solid #DFDFDF;vertical-align:middle;' name='{0}' id='{0}' data-val-required='Nhập vào mã an toàn' data-val-length-min='4' data-val-length-max='20' data-val-length='Bạn đã nhập sai mã an toàn.' data-val='true' class='input-text'></td>
<td><img alt='Mã captcha' src='/Ajax/Capcha/ShowCaptchaImage?r={2}&a={1}' id='img_{0}' style='vertical-align:middle;'></td></tr><tr><td colspan='2'><span data-valmsg-replace='true' data-valmsg-for='{0}' class='field-validation-valid'></span></td></tr></table>", id, a, 2));
            }  
        }

        /// <summary>
        /// Dùng trong trường hợp trên trang chỉ có 1 ô nhập captcha
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static MvcHtmlString SingleCaptcha(this HtmlHelper helper, string id)
        {
            Random rand = new Random();
            string a = rand.NextDouble().ToString();
            if (string.IsNullOrEmpty(id))
            {
                id = "Captcha";
            }
            
            return new MvcHtmlString(string.Format(@"<table cellpadding='0' cellspacing='0'><tr><td><input type='text' value='' style='width:100px;height:20px;border:1px solid #DFDFDF;vertical-align:middle;' name='{0}' id='{0}' data-val-required='Nhập vào mã an toàn' data-val-length-min='4' data-val-length-max='20' data-val-length='Bạn đã nhập sai mã an toàn.' data-val='true' class='input-text' onfocus=""$('#img_{0}').attr('src', '/Ajax/Capcha/ShowCaptchaImage?r=0&a=' + Math.random())""></td>
<td><img alt='Mã captcha' src='/Ajax/Capcha/ShowCaptchaImage?r=1&a={1}' id='img_{0}' style='vertical-align:middle;'></td></tr><tr><td colspan='2'><span data-valmsg-replace='true' data-valmsg-for='{0}' class='field-validation-valid'></span></td></tr></table>", id, a));
        }

        public static MvcHtmlString SingleCaptchaWithReset(this HtmlHelper helper, string id)
        {
            Random rand = new Random();
            string a = rand.NextDouble().ToString();
            if (string.IsNullOrEmpty(id))
            {
                id = "Captcha";
            }

            return new MvcHtmlString(string.Format(@"<table cellpadding='0' cellspacing='0'><tr><td><input type='text' value='' style='width:100px;height:20px;border:1px solid #DFDFDF;vertical-align:middle;' name='{0}' id='{0}' data-val-required='Nhập vào mã an toàn' data-val-length-min='4' data-val-length-max='20' data-val-length='Bạn đã nhập sai mã an toàn.' data-val='true' class='input-text' onfocus=""$('#img_{0}').attr('src', '/Ajax/Capcha/ShowCaptchaImage?r=0&a=' + Math.random())""></td>
<td><a class='resetCaptchaBtn' id='captchaReset' href='javascript:void(0);'></a></td><td><img alt='Mã captcha' src='/Ajax/Capcha/ShowCaptchaImage?r=1&a={1}' id='img_{0}' style='vertical-align:middle;'></td></tr><tr><td colspan='2'><span data-valmsg-replace='true' data-valmsg-for='{0}' class='field-validation-valid'></span></td></tr></table>", id, a));
        }
    }
}