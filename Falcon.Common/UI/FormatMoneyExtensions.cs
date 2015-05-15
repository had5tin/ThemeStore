using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Web.Mvc;

namespace Falcon.Common.UI
{
    public static class FormatMoneyExtensions
    {
        public static MvcHtmlString FormatMoney(this HtmlHelper helper, decimal? money, string zeroDisplay = "Thỏa thuận", int currencyType = 1)
        {
            string strMoney = zeroDisplay;
            if (money > 0)
            {
                string currency;
                if (currencyType == 2)
                {
                    currency = "USD";
                }
                else if (currencyType == 3)
                {
                    currency = "Lượng vàng";
                }else
                {
                    currency = "VNĐ";
                }
                strMoney = string.Format("{0:#,#} {1}", money, currency);                
            }
            
            return MvcHtmlString.Create(strMoney);
        }

        public static MvcHtmlString FormatMoneyWithSign(this HtmlHelper helper, decimal? money, string sign, string zeroDisplay = "Thỏa thuận", int currencyType = 1)
        {
            string strMoney = zeroDisplay;
            if (money > 0)
            {
                string currency;
                if (currencyType == 1)
                {
                    currency = "VNĐ";
                }
                else if (currencyType == 2)
                {
                    currency = "USD";
                }
                else if (currencyType == 3)
                {
                    currency = "Lượng vàng";
                }
                else
                {
                    currency = "";
                }
                strMoney = sign + string.Format("{0:#,#} {1}", money, currency);
            }

            return MvcHtmlString.Create(strMoney);
        }
    }
}
