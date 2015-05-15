using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Falcon.UI.Validation;

namespace Falcon.Modules.Contents.Models
{    
    public class StaticPageModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập vào tiêu đề trang")]
        [StringLength(255, ErrorMessage = "Tên tài khoản không dài quá {0} ký tự")]
        public string Title { get; set; }

        [StringLength(255, ErrorMessage = "Đường dẫn SEO không dài quá {0} ký tự")]
        public string SeoUrl { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không dài quá {0} ký tự")]        
        public string MetaDescription { get; set; }

        [StringLength(500, ErrorMessage = "Từ khóa không dài quá {0} ký tự")]
        public string MetaKeyword { get; set; }

        [Required(ErrorMessage = "Nhập vào nội dung trang")]
        [AllowHtml]
        public string Content { get; set; }

        public bool IsActive { get; set; }

        [StringLength(50, ErrorMessage = "Khung giao diện không dài quá {0} ký tự")]
        public string Layout { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}