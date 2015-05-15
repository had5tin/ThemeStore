using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Mvc.Controllers;
using Falcon.Themes;
using Falcon.Security;
using Falcon.Services.Users;
using Falcon.Data.Domain;

namespace Falcon.Admin.CoreModules.Controllers
{
    [Theme(ThemeType.Admin)]
    public class LoginController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserService _userService;

        public LoginController(
            IAuthenticationService authenticationService,
            IAuthorizationService authorizationService,
            IUserService userService)
        {
            this._authenticationService = authenticationService;
            this._authorizationService = authorizationService;
            this._userService = userService;
        }

        
        [Layout("Login")]
        public ActionResult Index()
        {
            if (!ValidateIpAddress())
            {
                return RedirectToAction("AccessDenied", new { area = "Users", controller = "Users" });
            }
            else
            {
                if (_authenticationService.GetAuthenticatedUser() != null)
                    return Redirect("~/admin");

                Title = "Đăng nhập quản trị hệ thống";
                return View();
            }            
        }

        [HttpPost]
        [Layout("Login")]
        public ActionResult Index(string username, string password)
        {
            if (!ValidateIpAddress())
            {
                return RedirectToAction("AccessDenied", new { area = "Users", controller = "Users" });
            }
            else
            {
                User user = ValidateUser(username, password);

                if (user == null)
                {
                    Title = "Đăng nhập quản trị hệ thống";
                    return View();
                }
                TouchIp();
                _authenticationService.SignIn(user, true);
                string returnUrl = Request.QueryString["returnUrl"] ?? "~/admin";
                return Redirect(returnUrl);
            }
        }

        public ActionResult Logout()
        {
            _authenticationService.SignOut();
            string returnUrl = Request.QueryString["returnUrl"] ?? "~/admin";
            return Redirect(returnUrl);       
        }

        [Layout("Login")]
        public ActionResult ForgotPassword()
        {
            Title = "Bạn quên tài khoản đăng nhập hay mật khẩu?";
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        [Layout("Login")]
        public ActionResult ResetIp()
        {
            Title = "Reset Ip truy xuất hệ thống quản trị";
            return View();
        }

        [HttpPost]
        [Layout("Login")]
        public ActionResult ResetIp(string username, string password)
        {
            User user = ValidateUser(username, password);

            if (user == null || (!user.ResetIpPermission && !user.UserName.Equals("root", StringComparison.InvariantCultureIgnoreCase)))
            {
                ErrorNotification("Thông tin đăng nhập không chính xác");
                Title = "Reset Ip truy xuất hệ thống quản trị";
                return View();
            }

            TouchIp();

            _authenticationService.SignIn(user, true);
            return Redirect("/admin");
        }

        #region Private Method
        private bool ValidateIpAddress()
        {
            string currentIp = GetRemoteIP();
            bool isAllow = false;
            if (IsPrivateIP(currentIp))
            {
                isAllow = true;
            }
            else
            {
                //check ip exist in Denied Directory
                if (!System.IO.File.Exists(Server.MapPath("/Ip/Banned/" + currentIp)))
                {
                    string filePath = Server.MapPath("/Ip/Allowed/" + currentIp);
                    if (System.IO.File.Exists(filePath))
                    {
                        var lastTouch = System.IO.File.GetLastWriteTime(filePath);
                        //1 địa chỉ Ip chỉ được tồn tại trong 1 ngày, sau đó cần phải xác minh lại
                        if (lastTouch.CompareTo(DateTime.Now.AddDays(-1)) > 0)
                        {
                            isAllow = true;
                        }
                    }
                }
            }
            return isAllow;
        }

        private void TouchIp()
        {
            string touchIp = Server.MapPath("/Ip/Allowed/" + GetRemoteIP());
            if (System.IO.File.Exists(touchIp))
            {
                System.IO.File.SetLastWriteTime(touchIp, DateTime.Now);
            }
            else
            {
                using (System.IO.FileStream fs = System.IO.File.Create(touchIp))
                {
                    //do nothing, just create an empty file
                }
            }
        }

        private User ValidateUser(string username, string password)
        {
            bool validate = true;

            if (String.IsNullOrEmpty(username))
            {
                //ModelState.AddModelError("username", "You must specify a username.");
                ErrorNotification("Nhập vào trường Tài Khoản");
                validate = false;
            }
            if (String.IsNullOrEmpty(password))
            {
                //ModelState.AddModelError("password", "You must specify a password.");
                ErrorNotification("Nhập vào trường Mật Khẩu");
                validate = false;
            }

            if (!validate)
                return null;

            var user = _userService.ValidateUser(username, password);
            if (user == null)
            {
                ErrorNotification("Thông tin đăng nhập không chính xác");
                //ModelState.AddModelError("_FORM", "The username or password provided is incorrect.");
            }

            return user;
        }
        #endregion
    }
}
