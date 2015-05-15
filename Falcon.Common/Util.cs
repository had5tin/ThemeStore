using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Web;
using Falcon.Infrastructure;
using System.Configuration;
using System.Web.Mvc;
using Falcon.Common.UI;
using System.Collections.Specialized;
using System.Web.Security;
using HtmlAgilityPack;

namespace Falcon.Common
{
    /// <summary>
    /// Chứa các hàm tiện ích dùng chung
    /// </summary>
    public class Util
    {
        public static string RemoveDoubleSlashes(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                url = url.Replace("http://", "http:");
                string pattern = @"//";
                var rgx = new Regex(pattern);
                url = rgx.Replace(url.Trim(), "/");
                url = url.Replace("http:", "http://");
            }
            return url;
        }

        public static void RefreshCaptcha()
        {
            HttpContext.Current.Session["captchastring"] = RandomCaptcha(4);
        }

        public static bool VerifyCaptcha(string captcha)
        {
            var sessionCaptcha = HttpContext.Current.Session["captchastring"];

            if (sessionCaptcha != null && !string.IsNullOrEmpty(sessionCaptcha.ToString()))
            {
                if (string.Compare(captcha, sessionCaptcha.ToString(), false) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetTime()
        {
            DateTime now = DateTime.Now;
            return now.Hour + " : " + now.Minute + " : " + now.Second;
        }

        public static string SubString(string input, int length, bool withDot = true)
        {
            string output = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Trim();
                int lengthInput = input.Length;
                if (lengthInput > length)
                {
                    input = input.Substring(0, length);
                    int lastSpace = input.LastIndexOf(' ');
                    if (lastSpace > 0)
                    {
                        output = input.Substring(0, lastSpace);
                    }
                    else
                    {
                        output = input;
                    }

                    if (withDot)
                    {
                        output = output.Trim() + " ...";
                    }
                }
                else
                    output = input;
            }
            return output;
        }
        public static string RandomCaptcha(int length)
        {
            int intZero = '0';
            int intNine = '9';
            int intA = 'a';
            int intZ = 'z';
            int intCount = 0;
            int intRandomNumber = 0;
            string strCaptchaString = "";

            var random = new Random(DateTime.Now.Millisecond);

            while (intCount < length)
            {
                intRandomNumber = random.Next(intZero, intZ);
                if (((intRandomNumber >= intZero) && (intRandomNumber <= intNine) || (intRandomNumber >= intA) && (intRandomNumber <= intZ)))
                {
                    strCaptchaString = strCaptchaString + (char)intRandomNumber;
                    intCount = intCount + 1;
                }
            }
            return strCaptchaString;
        }

        /// <summary>
        /// Tạo chuỗi ngẫu nhiên
        /// </summary>
        /// <param name="size"></param>
        /// <param name="useSpecialChars"></param>
        /// <returns></returns>
        public static string RandomString(int size, bool useSpecialChars = true)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            string chars;
            if (useSpecialChars)
            {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()<>?:;{}|.,/";
            }
            else
            {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            }

            var buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }
            return new string(buffer);
        }

        /// <summary>
        /// Tạo chuỗi ngẫu nhiên dùng làm Password Salt
        /// </summary>
        /// <param name="minSize"></param>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static string RandomPasswordSalt(int minSize = 5, int maxSize = 10)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            int size = random.Next(minSize, maxSize);
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            var buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = chars[random.Next(chars.Length)];
            }
            return new string(buffer);
        }

        /// <summary>
        /// Tạo chuỗi MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ComputeMD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return ByteToHexString(data);
            }
        }

        public static string ComputeHmacSHA1(string secretKey, string input)
        {
            var encoding = new UTF8Encoding();

            byte[] keyByte = encoding.GetBytes(secretKey);

            var hmacsha1 = new HMACSHA1(keyByte);

            byte[] messageBytes = encoding.GetBytes(input);
            byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);

            string encrypted = ByteToHexString(hashmessage);
            return encrypted;
        }

        public static string ByteToHexString(byte[] data)
        {
            var sBuilder = new StringBuilder();
            foreach (byte b in data)
            {
                sBuilder.Append(b.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string GenerateSecretKey()
        {
            string secretKey = RandomString(6, false);
            return ComputeHmacSHA1(secretKey, DateTime.Now.Ticks.ToString());
        }

        public static string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(toEncode);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFrom64(string toDecode)
        {
            byte[] decodeBytes = Convert.FromBase64String(toDecode);
            return Encoding.ASCII.GetString(decodeBytes);
        }

        /// <summary>
        /// Chuyển từ tiếng Việt có dấu => không dấu
        /// Ví dụ: Sàn giao dịch HàngTốt.com => San giao dich HangTot.com
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveVietnameseChar(string input)
        {
            string strResult;
            strResult = Regex.Replace(input, "[à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ]", "a");
            strResult = Regex.Replace(strResult, "[è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ]", "e");
            strResult = Regex.Replace(strResult, "[ì|í|ị|ỉ|ĩ]", "i");
            strResult = Regex.Replace(strResult, "[ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ]", "o");
            strResult = Regex.Replace(strResult, "[ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ]", "u");
            strResult = Regex.Replace(strResult, "[ỳ|ý|ỵ|ỷ|ỹ]", "y");
            strResult = Regex.Replace(strResult, "[đ]", "d");
            strResult = Regex.Replace(strResult, "[À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ]", "A");
            strResult = Regex.Replace(strResult, "[È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ]", "E");
            strResult = Regex.Replace(strResult, "[Ì|Í|Ị|Ỉ|Ĩ]", "I");
            strResult = Regex.Replace(strResult, "[Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ]", "O");
            strResult = Regex.Replace(strResult, "[Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ]", "U");
            strResult = Regex.Replace(strResult, "[Ỳ|Ý|Ỵ|Ỷ|Ỹ]", "Y");
            strResult = Regex.Replace(strResult, "[Đ]", "D");

            return strResult;
        }

        public static string GetSphinxVietnameseCharset(string input)
        {
            char[] normalChars = { ',', '-', '>', 'a', 'e', 'i', 'o', 'u', 'y', 'd' };
            string result = "";
            foreach (char c in input)
            {
                if (!normalChars.Contains(c))
                {
                    result += string.Format("U+{0:x4}", (int)c).ToUpper();
                }
                else
                {
                    result += c;
                }

            }
            return result;
        }

        public static string GetSphinxVietnameseCharset()
        {
            string result = "0..9,a..z,A..Z->a..z,_,";
            string a = "à,á,ạ,ả,ã,â,ầ,ấ,ậ,ẩ,ẫ,ă,ằ,ắ,ặ,ẳ,ẵ,";
            string A = "À->à,Á->á,Ạ->ạ,Ả->ả,Ã->ã,Â->â,Ầ->ầ,Ấ->ấ,Ậ->ậ,Ẩ->ẩ,Ẫ->ẫ,Ă->ă,Ằ->ằ,Ắ->ắ,Ặ->ặ,Ẳ->ẳ,Ẵ->ẵ,";
            string to_a = "à->a,á->a,ạ->a,ả->a,ã->a,â->a,ầ->a,ấ->a,ậ->a,ẩ->a,ẫ->a,ă->a,ằ->a,ắ->a,ặ->a,ẳ->a,ẵ->a,";

            string e = "è,é,ẹ,ẻ,ẽ,ê,ề,ế,ệ,ể,ễ,";
            string E = "È->è,É->é,Ẹ->ẹ,Ẻ->ẻ,Ẽ->ẽ,Ê->ê,Ề->ề,Ế->ế,Ệ->ệ,Ể->ể,Ễ->ễ,";
            string to_e = "è->e,é->e,ẹ->e,ẻ->e,ẽ->e,ê->e,ề->e,ế->e,ệ->e,ể->e,ễ->e,";

            string i = "ì,í,ị,ỉ,ĩ,";
            string I = "Ì->ì,Í->í,Ị->ị,Ỉ->ỉ,Ĩ->ĩ,";
            string to_i = "ì->i,í->i,ị->i,ỉ->i,ĩ->i,";

            string o = "ò,ó,ọ,ỏ,õ,ô,ồ,ố,ộ,ổ,ỗ,ơ,ờ,ớ,ợ,ở,ỡ,";
            string O = "Ò->ò,Ó->ó,Ọ->ọ,Ỏ->ỏ,Õ->õ,Ô->ô,Ồ->ồ,Ố->ố,Ộ->ộ,Ổ->ổ,Ỗ->ỗ,Ơ->ơ,Ờ->ờ,Ớ->ớ,Ợ->ợ,Ở->ở,Ỡ->ỡ,";
            string to_o = "ò->o,ó->o,ọ->o,ỏ->o,õ->o,ô->o,ồ->o,ố->o,ộ->o,ổ->o,ỗ->o,ơ->o,ờ->o,ớ->o,ợ->o,ở->o,ỡ->o,";

            string u = "ù,ú,ụ,ủ,ũ,ư,ừ,ứ,ự,ử,ữ,";
            string U = "Ù->ù,Ú->ú,Ụ->ụ,Ủ->ủ,Ũ->ũ,Ư->ư,Ừ->ừ,Ứ->ứ,Ự->ự,Ử->ử,Ữ->ữ,";
            string to_u = "ù->u,ú->u,ụ->u,ủ->u,ũ->u,ư->u,ừ->u,ứ->u,ự->u,ử->u,ữ->u,";

            string y = "ỳ,ý,ỵ,ỷ,ỹ,";
            string Y = "Ỳ->ỳ,Ý->ý,Ỵ->ỵ,Ỷ->ỷ,Ỹ->ỹ,";
            string to_y = "ỳ->y,ý->y,ỵ->y,ỷ->y,ỹ->y,";

            string d = "đ,";
            string D = "Đ->đ,";
            string to_d = "đ->d,";

            result += GetSphinxVietnameseCharset(a);
            result += GetSphinxVietnameseCharset(e);
            result += GetSphinxVietnameseCharset(i);
            result += GetSphinxVietnameseCharset(o);
            result += GetSphinxVietnameseCharset(u);
            result += GetSphinxVietnameseCharset(y);
            result += GetSphinxVietnameseCharset(d);
            result += GetSphinxVietnameseCharset(A);
            result += GetSphinxVietnameseCharset(E);
            result += GetSphinxVietnameseCharset(I);
            result += GetSphinxVietnameseCharset(O);
            result += GetSphinxVietnameseCharset(U);
            result += GetSphinxVietnameseCharset(Y);
            result += GetSphinxVietnameseCharset(D);
            result += GetSphinxVietnameseCharset(to_a);
            result += GetSphinxVietnameseCharset(to_e);
            result += GetSphinxVietnameseCharset(to_i);
            result += GetSphinxVietnameseCharset(to_o);
            result += GetSphinxVietnameseCharset(to_u);
            result += GetSphinxVietnameseCharset(to_y);
            result += GetSphinxVietnameseCharset(to_d);

            return result;

        }

        public const string SphinxAllowCharacter = "_0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzàáạảãâầấậẩẫăằắặẳẵÀÁẠẢÃÂẦẤẬẨẪĂẰẮẶẲẴèéẹẻẽêềếệểễÈÉẸẺẼÊỀẾỆỂỄìíịỉĩÌÍỊỈĨòóọỏõôồốộổỗơờớợởỡÒÓỌỎÕÔỒỐỘỔỖƠỜỚỢỞỠùúụủũưừứựửữÙÚỤỦŨƯỪỨỰỬỮỳýỵỷỹỲÝỴỶỸđĐ";

        public static string SphinxGetKeyword(string keyword)
        {
            return Regex.Replace(Regex.Replace(keyword, string.Format("[^{0}]", SphinxAllowCharacter), " ").Trim(), @"[ ]{2,}", " ").Trim();
        }
        /// <summary>
        /// Tạo Alias cho một chuỗi để SEO Url
        /// Ví dụ: Sàn giao dịch HàngTốt.com => San-giao-dich-HangTot-com
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetSEOAlias(string input, int maxLength = 0)
        {
            input = RemoveVietnameseChar(input);
            input = Regex.Replace(input, @"[^A-Za-z0-9]", "-");
            input = input.Trim('-');
            input = Regex.Replace(input, @"\-+", "-");
            if (maxLength > 80 || maxLength <= 0)
            {
                maxLength = 80;
            }
            if (maxLength > 0 && input.Length > maxLength)
            {
                input = input.Substring(0, maxLength);
            }
            return input.ToLower();
        }

        /// <summary>
        /// Kiểm tra Email có hợp lệ không
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>true - hợp lệ. fale - không hợp lệ</returns>
        public static bool ValidateEmail(string email)
        {
            string emailPattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*@((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            return Regex.IsMatch(email, emailPattern);
        }
        
        public static string GetHostFromUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                return uri.Host;
            }
            catch
            {
                return "";
            }

        }

        /// <summary>
        /// Format Template, fill data to Template.
        /// example: template = Hello {name}, object = new {name = "Khoi"}
        /// => result = Hello Khoi
        /// </summary>
        /// <param name="template"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormatTemplate(string template, object data)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(data))
            {
                string key = "{" + descriptor.Name + "}";
                if (!String.IsNullOrEmpty(template) && descriptor.GetValue(data) != null)
                {
                    template = template.Replace(key, descriptor.GetValue(data).ToString());
                }
            }
            return template;
        }

        public static int GetTimeStamp(DateTime date)
        {
            TimeSpan t = (date - new DateTime(1970, 1, 1));
            return (int)t.TotalSeconds;
        }

        public static double GetTimeStampMili(DateTime dt)
        {
            TimeSpan t = (dt - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return t.TotalMilliseconds;
        }

        public static int CurrentTimeStamp()
        {
            TimeSpan t = (DateTime.Now - new DateTime(1970, 1, 1));
            return (int)t.TotalSeconds;
        }

        public static long CurrentMicroTimeStamp()
        {
            TimeSpan t = (DateTime.Now - new DateTime(1970, 1, 1));
            return t.Ticks;
        }

        /// <summary>
        /// Hiển thị thời gian theo format giống Gmail
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <param name="seperate"></param>
        /// <returns></returns>
        public static string GetTimeDiff(DateTime date, string format = "{0:HH:mm | dd/MM/yyyy}", string seperate = "|")
        {
            string result = "";
            DateTime now = DateTime.Now;
            if (date > now)
            {
                return string.Format(format, date);
            }

            long totalMinutes = (long)now.Subtract(date).TotalMinutes;
            if (totalMinutes <= 1)
            {
                result = "1 phút trước";
            }
            else if (totalMinutes < 60)
            {
                result = string.Format("{0} phút trước", totalMinutes);
            }
            else if (totalMinutes < 1440)//24 * 60
            {
                int hours = (int)(totalMinutes / 60);
                int minute = (int)(totalMinutes - hours * 60);
                if (minute > 0)
                {
                    result = string.Format("{0} giờ {1} phút trước", hours, minute);
                }
                else
                {
                    result = string.Format("{0} giờ trước", hours);
                }

            }
            else if (totalMinutes < 2880)//48 * 60
            {
                result = string.Format("{0}:{1} {2} ngày hôm qua", date.Hour, date.Minute, seperate);
            }
            else
            {
                result = string.Format(format, date);
            }

            return result;
        }

        /// <summary>
        /// Trả về chuỗi dạng yyyyMMdd
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDateString(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                date = DateTime.Now;
            }

            return date.ToString("yyyyMMdd");
        }

        /// <summary>
        /// Trả về chuỗi dạng yyyyMMddHHmmss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDateTimeString(DateTime date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }

            return date.ToString("yyyyMMddHHmmss");
        }

        public static int GetCurrentCityId(HttpRequestBase request)
        {
            int result = 0;
            HttpCookie cookie = request.Cookies["City"];
            if (cookie != null && !String.IsNullOrEmpty(cookie.Value))
            {
                Int32.TryParse(cookie.Value, out result);
            }
            return result;
        }

        public static void SetCookie(HttpResponseBase response, string key, string value, DateTime expire)
        {
            var cookie = new HttpCookie(key, value);
            cookie.Expires = expire;
            response.Cookies.Add(cookie);
        }

        public static string GetThumbLink(string imagePath, ThumbSizeEnum thumbSize, bool includeMediaDomain = true)
        {
            string subPath = "";
            if (imagePath != null)
            {
                if (imagePath.StartsWith("~/"))
                {
                    // subPath = imagePath.Substring(1, imagePath.LastIndexOf("/"));
                    subPath = imagePath.Substring(1, imagePath.Length - 1);
                }
                else
                {
                    // subPath = imagePath.Substring(0, imagePath.LastIndexOf("/"));
                    subPath = imagePath;
                }
            }

            string currentPath = "/Thumbnail/" + thumbSize.ToString() + subPath;
            if (includeMediaDomain)
            {
                currentPath = EngineContext.Current.FalconConfig.MediaDomainName + currentPath;
            }

            return currentPath;
        }

        public static string GetImageLink(string imagePath, bool includeMediaDomain = true)
        {
            if (imagePath != null)
            {
                if (imagePath.StartsWith("~/"))
                {
                    imagePath = imagePath.Substring(1, imagePath.Length - 1);
                }
            }

            if (includeMediaDomain)
            {
                return EngineContext.Current.FalconConfig.MediaDomainName + imagePath;
            }
            
            return EngineContext.Current.FalconConfig.DomainName + imagePath;
        }

        /// <summary>
        /// Trả về chuỗi Indent dùng trong danh mục đa cấp
        /// Ví dụ:
        /// Danh mục cha<br/>
        /// └---Danh mục con<br/>
        /// └---Danh mục con 2<br/>
        /// └------Danh mục con 3<br/>
        /// </summary>
        /// <param name="depth">Độ sâu</param>
        /// <param name="startChars">Ký tự bắt đầu, mặc định là └</param>
        /// <param name="indentChars">Ký tự đánh dấu, mặc định là ---</param>
        /// <returns></returns>
        public static string GetIndent(int? depth, string startChars = "└", string indentChars = "---")
        {
            if (depth == null || depth == 0)
            {
                return "";
            }
            string indent = startChars;
            for (int i = 0; i < depth; i++)
            {
                indent += indentChars;
            }
            return indent;
        }
        public static string GetClassLevelMenu(int? depth)
        {
            if (depth == 0 || depth == null)
                return "level" + depth + " parent";
            
            return "level" + depth;
        }

        /// <summary>
        /// Clone 1 đối tượng ra 1 đối tượng giống hệt
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T Clone<T>(T o)
        {
            return ObjectCopier.Clone(o);
        }

        /// <summary>
        /// Bảo vệ dữ liệu (email, website) khỏi search engine & các tool crawl tự động
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ProtectWithJavascript(string s, string id)
        {
            string result = "<script type='text/javascript'>$(window).on('load', function(){$('#" + id + "').html(base64.decode('";
            result += EncodeTo64(s);
            result += "'));});</script>";

            return result;
        }

        public static string RemoveSpecialChar(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                string specialChars = @"[^\w\\_]";
                string multiSpacePattern = @"[ ]{2,}";
                s = Regex.Replace(s, specialChars, " ");
                s = Regex.Replace(s, multiSpacePattern, " ");
                s = s.Trim();
            }
            return s;
        }

        public static string FormatMoney(decimal? money, string currency = "VNĐ")
        {
            if (money == 0 || money == null)
            {
                return string.Format("0 {0}", currency);
            }
            return string.Format("{0:#,#} {1}", money, currency);
        }

        public static string FormatNumber(decimal? number)
        {
            if (number == null || number == 0) return "0";
            return string.Format("{0:#,#}", number);
        }

        /// <summary>
        /// Trả về tên loại tài khoản người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public static string GetAccountType(int id)
        //{
        //    switch (id)
        //    {
        //        case (int)AccountTypeEnum.Normal:
        //            return "Tài khoản thường";
        //        case (int)AccountTypeEnum.Store:
        //            return "Gian hàng thường";
        //        case (int)AccountTypeEnum.StorePremium:
        //            return "Gian hàng cao cấp";
        //        default:
        //            return string.Format("Không xác định [{0}]", id);
        //    }
        //}

        //public static string GetAccountTypeName(int id)
        //{
        //    switch (id)
        //    {
        //        case (int)AccountTypeEnum.Normal:
        //            return "TK thường";
        //        case (int)AccountTypeEnum.Store:
        //            return "GH thường";
        //        case (int)AccountTypeEnum.StorePremium:
        //            return "GH cao cấp";
        //        default:
        //            return string.Format("Không xác định [{0}]", id);
        //    }
        //}

        /// <summary>
        /// Trả về tên loại tài khoản người dùng
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        //public static string GetAccountType(AccountTypeEnum id)
        //{
        //    switch (id)
        //    {
        //        case AccountTypeEnum.Normal:
        //            return "Tài khoản thường";
        //        case AccountTypeEnum.Store:
        //            return "Gian hàng thường";
        //        case AccountTypeEnum.StorePremium:
        //            return "Gian hàng cao cấp";
        //        default:
        //            return string.Format("Không xác định [{0}]", id);
        //    }
        //}

        /// <summary>
        /// Trả về SelectList danh sách các loại tài khoản giao dịch
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        //public static SelectList GetListAccountTypes(int? selected)
        //{
        //    var typelist = new List<object>() 
        //    { 
        //        new { Text = Util.GetAccountType((int)AccountTypeEnum.Normal), Value = (int)AccountTypeEnum.Normal },
        //        new { Text = Util.GetAccountType((int)AccountTypeEnum.Store), Value = (int)AccountTypeEnum.Store },
        //        new { Text = Util.GetAccountType((int)AccountTypeEnum.StorePremium), Value = (int)AccountTypeEnum.StorePremium }
        //    };

        //    return new SelectList(typelist, "Value", "Text", selected);
        //}

        //public static CheckBoxList GetCheckBoxListAccountTypes(string id, List<int> selectedAccounts)
        //{
        //    var result = new CheckBoxList();

        //    result.Id = id;

        //    result.Items.Add(GetAccountCheckboxListItem(AccountTypeEnum.Normal, selectedAccounts));
        //    result.Items.Add(GetAccountCheckboxListItem(AccountTypeEnum.Store, selectedAccounts));
        //    result.Items.Add(GetAccountCheckboxListItem(AccountTypeEnum.StorePremium, selectedAccounts));

        //    return result;
        //}

        //private static CheckBoxListItem GetAccountCheckboxListItem(AccountTypeEnum type, List<int> selectedAccounts)
        //{
        //    int val = (int)type;
        //    return new CheckBoxListItem()
        //    {
        //        Value = val.ToString(),
        //        Text = GetAccountType(type),
        //        Checked = (selectedAccounts != null && selectedAccounts.Contains(val)) ? true : false
        //    };
        //}

        /// <summary>
        /// Trả về tên loại tài khoản người dùng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public static string GetAccountStatus(int id)
        //{
        //    switch (id)
        //    {
        //        case (int)AccountStatusEnum.Active:
        //            return "Hoạt động";
        //        case (int)AccountStatusEnum.NotVerifyEmail:
        //            return "Chưa kích hoạt email";
        //        case (int)AccountStatusEnum.Locked:
        //            return "Bị khóa";
        //        case (int)AccountStatusEnum.Deleted:
        //            return "Bị xóa";
        //        case (int)AccountStatusEnum.VerifiedInfo:
        //            return "Xác minh";
        //        default:
        //            return string.Format("Không xác định [{0}]", id);
        //    }
        //}

        /// <summary>
        /// Trả về SelectList danh sách các loại tài khoản giao dịch
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        //public static SelectList GetListAccountStatuses(int selected)
        //{
        //    var typelist = new List<object>() 
        //    { 
        //        new { Text = Util.GetAccountStatus((int)AccountStatusEnum.Active), Value = (int)AccountStatusEnum.Active },
        //        new { Text = Util.GetAccountStatus((int)AccountStatusEnum.NotVerifyEmail), Value = (int)AccountStatusEnum.NotVerifyEmail },
        //        new { Text = Util.GetAccountStatus((int)AccountStatusEnum.Locked), Value = (int)AccountStatusEnum.Locked },
        //        new { Text = Util.GetAccountStatus((int)AccountStatusEnum.Deleted), Value = (int)AccountStatusEnum.Deleted },
        //        new { Text = Util.GetAccountStatus((int)AccountStatusEnum.VerifiedInfo), Value = (int)AccountStatusEnum.VerifiedInfo }
        //    };

        //    return new SelectList(typelist, "Value", "Text", selected);
        //}

        public class DateDifference
        {
            /// <summary>
            /// defining Number of days in month; index 0=> january and 11=> December
            /// february contain either 28 or 29 days, that's why here value is -1
            /// which wil be calculate later.
            /// </summary>
            private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            /// <summary>
            /// contain from date
            /// </summary>
            private DateTime fromDate;

            /// <summary>
            /// contain To Date
            /// </summary>
            private DateTime toDate;

            /// <summary>
            /// this three variable for output representation..
            /// </summary>
            private int year;
            private int month;
            private int day;

            public DateDifference(DateTime d1, DateTime d2)
            {
                int increment;

                if (d1 > d2)
                {
                    this.fromDate = d2;
                    this.toDate = d1;
                }
                else
                {
                    this.fromDate = d1;
                    this.toDate = d2;
                }

                /// 
                /// Day Calculation
                /// 
                increment = 0;

                if (this.fromDate.Day > this.toDate.Day)
                {
                    increment = this.monthDay[this.fromDate.Month - 1];

                }
                /// if it is february month
                /// if it's to day is less then from day
                if (increment == -1)
                {
                    if (DateTime.IsLeapYear(this.fromDate.Year))
                    {
                        // leap year february contain 29 days
                        increment = 29;
                    }
                    else
                    {
                        increment = 28;
                    }
                }
                if (increment != 0)
                {
                    day = (this.toDate.Day + increment) - this.fromDate.Day;
                    increment = 1;
                }
                else
                {
                    day = this.toDate.Day - this.fromDate.Day;
                }

                ///
                ///month calculation
                ///
                if ((this.fromDate.Month + increment) > this.toDate.Month)
                {
                    this.month = (this.toDate.Month + 12) - (this.fromDate.Month + increment);
                    increment = 1;
                }
                else
                {
                    this.month = (this.toDate.Month) - (this.fromDate.Month + increment);
                    increment = 0;
                }

                ///
                /// year calculation
                ///
                this.year = this.toDate.Year - (this.fromDate.Year + increment);

            }

            public override string ToString()
            {
                //return base.ToString();
                return this.year + " Year(s), " + this.month + " month(s), " + this.day + " day(s)";
            }

            public int Years
            {
                get
                {
                    return this.year;
                }
            }

            public int Months
            {
                get
                {
                    return this.month;
                }
            }

            public int Days
            {
                get
                {
                    return this.day;
                }
            }

        }

        /// <summary>
        /// retrieves the IP address of the current request -- handles proxies and private networks
        /// </summary>
        public static string GetRemoteIP(NameValueCollection ServerVariables)
        {
            var _ipAddress = new Regex(@"\b([0-9]{1,3}\.){3}[0-9]{1,3}$",
                                                             RegexOptions.Compiled | RegexOptions.ExplicitCapture);

            string ip = ServerVariables["REMOTE_ADDR"]; // could be a proxy -- beware
            string ipForwarded = ServerVariables["HTTP_X_FORWARDED_FOR"];

            // check if we were forwarded from a proxy
            if (!string.IsNullOrEmpty(ipForwarded))
            {
                ipForwarded = _ipAddress.Match(ipForwarded).Value;
                if (!string.IsNullOrEmpty(ipForwarded) && !IsPrivateIP(ipForwarded))
                    ip = ipForwarded;
            }

            return !string.IsNullOrEmpty(ip) ? ip : "0.0.0.0";
        }

        /// <summary>
        /// returns true if this is a private network IP  
        /// http://en.wikipedia.org/wiki/Private_network
        /// </summary>
        public static bool IsPrivateIP(string s)
        {
            return (s.StartsWith("192.168.") || s.StartsWith("10.") || s.StartsWith("127.0.0."));
        }

        /// <summary>
        /// Trả về SelectList danh sách bản ghi trong trang
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static SelectList GetListRowsPerPage(int? selected)
        {
            var typelist = new List<object>() 
            { 
                new { Text = "10 Bản ghi / trang", Value = 10  },
                new { Text = "20 Bản ghi / trang", Value = 20  },
                new { Text = "30 Bản ghi / trang", Value = 30  },
                new { Text = "50 Bản ghi / trang", Value = 50  },
                new { Text = "100 Bản ghi / trang", Value = 100  }
            };

            return new SelectList(typelist, "Value", "Text", selected);
        }

        public static DateTime RandomDateTime(DateTime from, DateTime to)
        {
            Random gen = new Random();
            int range = (to - from).Days;
            return from.AddDays(gen.Next(range)).AddSeconds(new Random().Next(86400));
        }

        public static DateTime RandomDatetimeInDay(DateTime from, DateTime to)
        {
            Random rnd = new Random();
            var range = to - from;
            var randTimeSpan = new TimeSpan((long)(rnd.NextDouble() * range.Ticks));
            return from + randTimeSpan;
        }

        public static string RemoveNewLine(string input, string replacement)
        {
            return Regex.Replace(input, @"\r\n?|\n", replacement);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="dayOfWeek"></param>
        /// <returns>Time của ngày gần nhất</returns>
        public static DateTime GetNextDate(DateTime from, int dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            if (dayOfWeek < start)
                dayOfWeek += 7;
            return from.AddDays(dayOfWeek - start);
        }

        /// <summary>
        /// Kiểm tra 2 đoạn [a,b] giao với [c,d] không
        /// Nếu b=c thì vẫn hợp lệ
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static bool IsIntersection(int a, int b, int c, int d)
        {
            if (a > c)
            {
                if (d <= a)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (a < c)
            {
                if (b <= c)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        //public static List<Model.MenuModel> AddParent(List<Model.MenuModel> menuList)
        //{
        //    foreach (var item in menuList)
        //    {
        //        _AddParrent(item);
        //    }
        //    return menuList;
        //}

        //private static Model.MenuModel _AddParrent(Model.MenuModel menuModel)
        //{
        //    if (menuModel.SubMenu != null && menuModel.SubMenu.Count > 0)
        //    {
        //        foreach (var item in menuModel.SubMenu)
        //        {
        //            item.Parent = menuModel;
        //            _AddParrent(item);
        //        }
        //    }
        //    return menuModel;
        //}

        //public static bool CheckCurrentMenu(Model.MenuModel menu, Model.RouteModel currentRoute)
        //{
        //    if (menu.ListRoute != null && menu.ListRoute.Count > 0)
        //    {
        //        foreach (var route in menu.ListRoute)
        //        {
        //            if (route.CompareWith(currentRoute))
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (menu.SubMenu != null && menu.SubMenu.Count > 0)
        //        {
        //            foreach (var submenu in menu.SubMenu)
        //            {
        //                if (CheckCurrentMenu(submenu, currentRoute))
        //                    return true;
        //            }
        //        }
        //    }

        //    return false;
        //}

        //public static List<Model.MenuModel> SetCurrentMenu(List<Model.MenuModel> menuList, Model.RouteModel currentRoute)
        //{
        //    var currentMenu = _FindCurrentMenu(menuList, currentRoute);
        //    do
        //    {
        //        if (currentMenu != null)
        //        {
        //            currentMenu.Active = true;
        //            currentMenu = currentMenu.Parent;
        //        }
        //    }
        //    while (currentMenu != null);
        //    return menuList;
        //}

        //private static Model.MenuModel _FindCurrentMenu(List<Model.MenuModel> menuList, Model.RouteModel currentRoute)
        //{
        //    foreach (var item in menuList)
        //    {
        //        if (item.ListRoute != null && item.ListRoute.Count > 0)
        //        {
        //            foreach (var route in item.ListRoute)
        //            {
        //                if (route.CompareWith(currentRoute))
        //                {
        //                    return item;
        //                }
        //            }
        //        }

        //        if (item.SubMenu == null || item.SubMenu.Count == 0)
        //        {

        //        }
        //        else
        //        {
        //            var temp = _FindCurrentMenu(item.SubMenu, currentRoute);
        //            if (temp != null)
        //                return temp;
        //        }
        //    }

        //    return null;
        //}

        //public static List<Model.MenuModel> GetLineageCurrentMenu(List<Model.MenuModel> menuList, Model.RouteModel currentRoute)
        //{
        //    var lineageMenuList = new List<Model.MenuModel>();
        //    var currentMenu = _FindCurrentMenu(menuList, currentRoute);
        //    do
        //    {
        //        if (currentMenu != null)
        //        {
        //            lineageMenuList.Add(currentMenu);
        //            currentMenu = currentMenu.Parent;
        //        }
        //    }
        //    while (currentMenu != null);

        //    return lineageMenuList;
        //}

        public static int GetCurrentAccountId()
        {
            var iAccountId = 0;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var userData = authTicket.UserData;
                if (!string.IsNullOrEmpty(userData) && userData.StartsWith("account-"))
                {
                    int.TryParse(userData.Substring(8), out iAccountId);
                }
            }
            return iAccountId;
        }

        public static string GetDayOfWeek(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return "Chủ nhật";
                case DayOfWeek.Monday:
                    return "Thứ Hai";
                case DayOfWeek.Tuesday:
                    return "Thứ Ba";
                case DayOfWeek.Wednesday:
                    return "Thứ Tư";
                case DayOfWeek.Thursday:
                    return "Thứ Năm";
                case DayOfWeek.Friday:
                    return "Thứ Sáu";
                case DayOfWeek.Saturday:
                    return "Thứ Bảy";
                default: return string.Empty;
            }
        }

        public static readonly char[] Alphabet = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'Ă', 'Â', 'B', 'C', 'D', 'Đ', 'E', 'Ê', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'Ô', 'Ơ', 'P', 'Q', 'R', 'S', 'T', 'U', 'Ư', 'V', 'X', 'Y' };

        public static decimal FormatDecimal(decimal? number)
        {
            if (number == null)
            {
                return 0;
            }
            
            return Convert.ToDecimal(number.Value.ToString("0"));
        }


        private static readonly Dictionary<string, string[]> ValidHtmlTags =
            new Dictionary<string, string[]>
        {
            {"p", new string[]          {}},
            {"div", new string[]        {}},
            {"br", new string[]         {}},
            {"hr", new string[]         {}},
            {"h1", new string[]         {}},
            {"h2", new string[]         {}},
            {"h3", new string[]         {}},
            {"h4", new string[]         {}},
            {"h5", new string[]         {}},
            {"h6", new string[]         {}},
            {"strong", new string[]     {}},
            {"b", new string[]          {}},
            {"em", new string[]         {}},
            {"i", new string[]          {}},
            {"u", new string[]          {}},
            {"strike", new string[]     {}},
            {"ol", new string[]         {}},
            {"ul", new string[]         {}},
            {"li", new string[]         {}},
            {"blockquote", new string[] {}},
            {"code", new string[]       {}},
            {"a", new string[]          { "href", "title", "rel", "target"}},
            {"img", new string[]        {"src","alt", "title"}},
            {"table", new string[]      {}},
            {"thead", new string[]      {}},
            {"tbody", new string[]      {}},
            {"tfoot", new string[]      {}},
            {"th", new string[]         {}},
            {"tr", new string[]         {}},
            {"td", new string[]         {"colspan", "align", "valign"}},
            {"q", new string[]          {}},
            {"cite", new string[]       {}},
            {"abbr", new string[]       {}},
            {"acronym", new string[]    {}},
            {"del", new string[]        {}},
            {"ins", new string[]        {}},
            {"article", new string[]    {}},
            //Thêm mới cho link iframe Youtube
            {"iframe", new[] {"width", "height", "src", "frameborder"}},
        };
        private static void RemoveElementKeepText(HtmlNode node)
        {
            //node.ParentNode.RemoveChild(node, true);
            HtmlNode parent = node.ParentNode;
            HtmlNode prev = node.PreviousSibling;
            HtmlNode next = node.NextSibling;

            foreach (HtmlNode child in node.ChildNodes)
            {
                if (prev != null)
                    parent.InsertAfter(child, prev);
                else if (next != null)
                    parent.InsertBefore(child, next);
                else
                    parent.AppendChild(child);

            }
            node.Remove();
        }

        public static string GetMobileContent(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }
            var htmlDoc = new HtmlDocument();
            var allowTagList = ValidHtmlTags.Keys;
            // load html
            htmlDoc.LoadHtml(input);
            var nodeList = htmlDoc.DocumentNode.SelectNodes("//*");
            if (nodeList != null)
            {
                foreach (var node in nodeList)
                {
                    if (allowTagList.Contains(node.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        var allowAtribute = ValidHtmlTags[node.Name];
                        var attributes = node.Attributes;
                        if (attributes != null)
                        {
                            var notAllowList = attributes.Where(a => !allowAtribute.Contains(a.Name, StringComparer.OrdinalIgnoreCase)).ToList();
                            foreach (var item in notAllowList)
                            {
                                attributes.Remove(item.Name);
                            }
                        }
                    }
                    else
                    {
                        RemoveElementKeepText(node);
                    }
                }
            }
            return htmlDoc.DocumentNode.WriteContentTo().Trim();
        }


        //public static int DectectMobileDevice(string userAgent)
        //{
        //    userAgent = userAgent.ToLower();
        //    if (Regex.IsMatch(userAgent, "android"))
        //    {
        //        return MobileDeviceTypeConst.Android;
        //    }
        //    if (Regex.IsMatch(userAgent, "blackberry"))
        //    {
        //        return MobileDeviceTypeConst.BlackBerry;
        //    }
        //    if (Regex.IsMatch(userAgent, "ip(hone|od|ad)"))
        //    {
        //        return MobileDeviceTypeConst.IOS;
        //    }
        //    if (Regex.IsMatch(userAgent, "windows (ce|phone)"))
        //    {
        //        return MobileDeviceTypeConst.Winphone;
        //    }
        //    return MobileDeviceTypeConst.Other;
        //}

        public static string GetRedisConfig()
        {
            return ConfigurationManager.AppSettings["RedisConfig"] != null ? ConfigurationManager.AppSettings["RedisConfig"] : "localhost";
        }
    }
}
