using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace Falcon.Common.UI
{
    public static class InputExtensions
    {
        public static MvcHtmlString TextBoxNumber(this HtmlHelper helper, string name, decimal? value, object htmlAttributes = null, string currency = "VNĐ")
        {
            return CreateInputNumber(name, value, htmlAttributes, currency);
        }

        internal static MvcHtmlString CreateInputNumber(string name, decimal? value, object htmlAttributes, string currency)
        {
            string script = string.Format(@"<script type='text/javascript'>
					$(document).ready(function(){{
						$('#{0}').bind('keyup', function(){{
							initFormatNumber_{0}();	}});
						initFormatNumber_{1}();
					}});
					function initFormatNumber_{1}(){{
						$('#detail_{0}').text($('#{0}').val());						
						if($('#{0}').val() != '') {{
							$('#detail_{0}').formatNumber({{format:'#,###', locale:'vn'}});
						}}}}function numbersonly(myfield, e, dec)
							{{
								var key;
								var keychar;								
								if (window.event)
								   key = window.event.keyCode;
								else if (e)
								   key = e.which;
								else
								   return true;
								keychar = String.fromCharCode(key);
								if ((key==null) || (key==0) || (key==8) || 
								    (key==9) || (key==13) || (key==27) )
								   return true;
								else if ((('0123456789').indexOf(keychar) > -1))
								   return true;
								else if (dec && (keychar == '.'))
								   {{
								   myfield.form.elements[dec].focus();
								   return false;
								   }}
								else
								   return false;
							}}</script>", name, name.Replace(" ", ""));  
            
            var builder = new TagBuilder("input");
            builder.MergeAttribute("id", name);
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("type", "text");
            builder.MergeAttribute("value", value == null ? "" : string.Format("{0:G29}", decimal.Parse(value.ToString())));            
            builder.MergeAttribute("autocomplete", "off");
            builder.MergeAttribute("style", "text-align:right");
            builder.MergeAttribute("onkeypress", "return numbersonly(this, event)");            
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);

            string divDetail = string.Format(@" {0}<div id='detail_{1}' style='font-weight:bold; padding-right:3px;'></div>", currency, name);

            return new MvcHtmlString(script + builder.ToString() + divDetail);
            
        }
    }
}
