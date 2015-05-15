using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Falcon.UI.Validation;
using System.ComponentModel;

namespace Falcon.Admin.CoreModules.Models
{
    public class SystemSettingModel
    {
        [Required(ErrorMessage = "Không được để trống")]
        [StringLength(50, ErrorMessage = "Không được vượt quá {0} ký tự")]
        [DisplayName("Key")]
        public string SettingKey { get; set; }

        [StringLength(100, ErrorMessage = "Không được vượt quá {0} ký tự")]
        [DisplayName("Tiêu đề")]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "Không được vượt quá {0} ký tự")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Kiểu hiển thị")]
        public int Type { get; set; }

        [DisplayName("Thiết lập giá trị")]
        [AllowHtml]
        public string Value { get; set; }

        [StringLength(255, ErrorMessage = "Không được vượt quá {0} ký tự")]
        [DisplayName("Danh sách giá trị")]
        public string Options { get; set; }

        [DisplayName("Được phép để trắng")]
        public bool IsRequired { get; set; }

        public SelectList SystemSettingType { get; set; }
        public List<string> ListSelected { get; set; }
        public SelectList ListOptions { get; set; }
    }
}