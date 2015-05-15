using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Falcon.UI.Validation;
using Falcon.Data.Domain;
using Falcon.Mvc;

namespace Falcon.Admin.CoreModules.Models
{
    public class ThemeModel
    {
        public string VirtualPath { get; set; }

        public string ThemeName { get; set; }

        public string ThemeTitle { get; set; }

        public DateTime Modified { get; set; }

        public string ThemeType { get; set; }

        public bool IsStoreTheme { get; set; }
    }
}