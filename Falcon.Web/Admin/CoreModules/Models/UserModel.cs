using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Falcon.UI.Validation;

namespace Falcon.Admin.CoreModules.Models
{    
    public class CreateUserModel
    {
        [Required(ErrorMessage = "Nhập vào tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không dài quá 50 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nhập vào địa chỉ email")]
        [StringLength(128, ErrorMessage = "Địa chỉ email không dài quá 128 ký tự")]
        [DataType(DataType.EmailAddress)]
        [Email(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nhập vào mật khẩu")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu dài từ 6 đến 50 ký tự")]
        public string Password { get; set; }

        [DataType(DataType.Password)]        
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu không khớp nhau")]        
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Nhập vào tên người dùng")]
        [StringLength(100, ErrorMessage = "Tên người dùng dài không quá 100 ký tự")]
        public string FullName { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ dài không quá 255 ký tự")]
        public string Address { get; set; }

        [StringLength(255, ErrorMessage = "Mô tả dài không quá 255 ký tự")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public List<int> RoleId { get; set; }
        public MultiSelectList Roles { get; set; }
    }

    //[Bind(Exclude = "Id")]
    public class EditUserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập vào tên đăng nhập")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không dài quá 50 ký tự")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nhập vào địa chỉ email")]
        [StringLength(128, ErrorMessage = "Địa chỉ email không dài quá 128 ký tự")]
        [DataType(DataType.EmailAddress)]
        [Email(ErrorMessage = "Email không đúng định dạng")]
        [Display(Name = "Địa chỉ Email")]
        public string Email { get; set; }        

        [Required(ErrorMessage = "Nhập vào tên người dùng")]
        [StringLength(100, ErrorMessage = "Tên người dùng dài không quá 100 ký tự")]
        [Display(Name = "Tên người dùng")]
        public string FullName { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ dài không quá 255 ký tự")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [StringLength(255, ErrorMessage = "Mô tả dài không quá 255 ký tự")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Hoạt động")]
        public bool IsActive { get; set; }

        [Display(Name = "Quyền Reset Ip")]
        public bool ResetIpPermission { get; set; }

        [StringLength(100, ErrorMessage = "Mật khẩu dài từ 6 đến 50 ký tự", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Mật khẩu mới không khớp nhau")]        
        public string ConfirmPassword { get; set; }

        public List<int> RoleId { get; set; }

        [DisplayName("Vai trò")]
        public MultiSelectList Roles { get; set; }
    }

    public class ChangePasswordUserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nhập vào mật khẩu cũ")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu dài từ 6 đến 50 ký tự")]
        [DataType(DataType.Password)]       
        [DisplayName("Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Nhập vào mật khẩu mới")]
        [StringLength(100, ErrorMessage = "Mật khẩu dài từ 6 đến 50 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu mới")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu mới không khớp nhau")]
        [DisplayName("Nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }        
    }

    public class ChangePasswordUserAdminModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Nhập vào mật khẩu mới")]
        [StringLength(100, ErrorMessage = "Mật khẩu dài từ 6 đến 50 ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu mới")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu mới không khớp nhau")]
        [DisplayName("Nhập lại mật khẩu")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginUserModel
    {
        [Required(ErrorMessage = "Nhập vào tên tài khoản")]        
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nhập vào mật khẩu")]        
        public string Password { get; set; }
        
        public bool RememberMe { get; set; }
    }
}