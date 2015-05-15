using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Falcon.UI.Validation;
using Falcon.Data.Domain;
using Falcon.Mvc;

namespace Falcon.Admin.CoreModules.Models
{    
    public class RoleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập vào tên vai trò")]
        [StringLength(64, ErrorMessage = "Tên vai trò không dài quá {0} ký tự")]
        [DisplayName("Tên vai trò")]
        public string Name { get; set; }
        
        [StringLength(256, ErrorMessage = "Tên người dùng dài không quá {0} ký tự")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [NotEqualsTo("Id", ErrorMessage = "Vai trò cha trùng với vai trò hiện tại")]
        [DisplayName("Vai trò cha")]
        public int? ParentId { get; set; }

        public SelectList RoleList { get; set; }
    }   
}