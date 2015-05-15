using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Falcon.Admin.CoreModules.Models
{
    public class JsTreeModel
    {
        public JsTreeModel()
        {
            children = new List<JsTreeModel>();
            attr = new JsTreeAttribute();
        }

        public string data;
        public JsTreeAttribute attr;
        public List<JsTreeModel> children;
    }

    public class JsTreeAttribute
    {
        public string id;
        public bool selected;
    }
}
