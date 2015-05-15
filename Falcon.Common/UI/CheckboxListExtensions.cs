using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Falcon.Common.UI
{
    public static class CheckboxListExtensions
    {
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, CheckBoxList checkBoxList)
        {
            var builder = new StringBuilder();

            if (checkBoxList.Items.Count > 0)
            {
                foreach (var item in checkBoxList.Items)
                {
                    builder.Append(string.Format("<input type='checkbox' id='{0}' name='{1}' {2} value='{3}'/><label for='{0}'>{4}</label><br/>", checkBoxList.Id + "_" + item.Value, checkBoxList.Id, item.Checked ? "checked='checked'" : "", item.Value, item.Text));
                }

                return new MvcHtmlString(builder.ToString());
            }

            return new MvcHtmlString("");
        }
    }

    public class CheckBoxList
    {
        public string Id { get; set; }
        public List<CheckBoxListItem> Items { get; set; }

        public CheckBoxList()
        {
            Items = new List<CheckBoxListItem>();
        }
    }

    public class CheckBoxListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }        
        public bool Checked { get; set; }
    }
}
