using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Falcon.Mvc.Controllers;
using Falcon.Security;

using Falcon.Services.Users;
using Falcon.Data.Domain;
using Falcon.Admin.CoreModules.Models;
using Falcon.UI.Html.Admin;
using Falcon.Common;
using Falcon.Common.UI;

namespace Falcon.Admin.CoreModules.Controllers
{
    public class UserController : AdminBaseController
    {
        private readonly IUserService userService;
        private readonly IAuthenticationService authenService;
        private readonly IAuthorizationService authorService;

        public UserController(IUserService userService, IAuthenticationService authService, IAuthorizationService authorService)
        {
            this.userService = userService;
            this.authenService = authService;
            this.authorService = authorService;
        }
        //
        // GET: /Admin/User/

        public ActionResult Index(int page = 1)
        {
            Title = "Quản trị tài khoản hệ thống";
            int pageSize = this.systemSettingService.Get<int>("PageSizeAdmin", 10);
            var PagerModels = new PaginationModels
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalRecords = userService.GetAllCount()
            };
            ViewData["PagerModels"] = PagerModels;
            IEnumerable<User> users = userService.GetAll(page, pageSize);
            return View(users);
        }

        //
        // GET: /Admin/User/Details/5

        public ActionResult Details(int id)
        {
            Title = "Chi tiết tài khoản hệ thống";
            return View();
        }

        //
        // GET: /Admin/User/Create

        public ActionResult Create()
        {
            Title = "Thêm mới tài khoản hệ thống";

            CreateUserModel model = new CreateUserModel();

            IEnumerable<Role> roles = authorService.GetAllRoles();
            model.Roles = new MultiSelectList(roles, "Id", "Name");

            return View(model);
        }

        //
        // POST: /Admin/User/Create

        [HttpPost]
        public ActionResult Create(CreateUserModel userModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = userModel.ToEntity();
                    user.PasswordSalt = Util.RandomPasswordSalt();
                    user.Password = Util.ComputeMD5Hash(user.Password + user.PasswordSalt);
                    user.Created = DateTime.Now;
                    user.Modified = user.Created;

                    userService.AddUser(user);

                    if (userModel.RoleId != null)
                    {
                        foreach (int roleId in userModel.RoleId)
                        {
                            authorService.AddUserRole(user.Id, roleId);
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ErrorNotification(e);
                }
            }
            else
            {
                AddModelStateErrors();
            }

            IEnumerable<Role> roles = authorService.GetAllRoles();
            userModel.Roles = new MultiSelectList(roles, "Id", "Name");

            Title = "Thêm mới tài khoản hệ thống";
            return View(userModel);
        }

        //
        // GET: /Admin/User/Edit/5

        public ActionResult Edit(int id)
        {
            User user = userService.GetUserForUpdate(id);
            if (user == null || (user.Id == 1 && WorkContext.CurrentUserId != 1))
            {
                ErrorNotification("Không tìm thấy tài khoản nào thỏa mãn");
                return RedirectToAction("Index");
            }
            EditUserModel model = user.ToEditModel();

            IEnumerable<Role> roles = authorService.GetAllRoles();
            IEnumerable<Role> selectedRoles = authorService.GetAllRolesByUser(id);
            model.Roles = new MultiSelectList(roles, "Id", "Name", selectedRoles.Select(r => r.Id).ToList());

            Title = "Chỉnh sửa thông tin tài khoản " + user.FullName;
            return View(model);
        }

        //
        // POST: /Admin/User/Edit/5

        [HttpPost]
        public ActionResult Edit(EditUserModel userModel)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case ButtonActionName.Save:
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            User user = userService.GetUserForUpdate(userModel.Id);
                            if (user == null && (user.Id == 1 && WorkContext.CurrentUserId != 1))
                            {
                                ErrorNotification("Không tìm thấy tài khoản nào thỏa mãn");
                                return RedirectToAction("Index");
                            }
                            user.Email = userModel.Email;
                            user.UserName = userModel.UserName;
                            user.Address = userModel.Address;
                            user.Description = userModel.Description;
                            user.FullName = userModel.FullName;
                            user.IsActive = userModel.IsActive;
                            user.ResetIpPermission = userModel.ResetIpPermission;
                            user.Modified = DateTime.Now;

                            userService.UpdateUser(user);

                            authorService.RemoveUserRoleByUserId(userModel.Id);
                            if (userModel.RoleId != null)
                            {
                                foreach (int roleId in userModel.RoleId)
                                {
                                    authorService.AddUserRole(userModel.Id, roleId);
                                }
                            }

                            SuccessNotification("Cập nhật thông tin tài khoản thành công");
                            return RedirectToAction("Index");
                        }
                        catch (Exception e)
                        {
                            ErrorNotification(e);
                        }
                    }

                    IEnumerable<Role> roles = authorService.GetAllRoles();
                    IEnumerable<Role> selectedRoles = authorService.GetAllRolesByUser(userModel.Id);
                    userModel.Roles = new MultiSelectList(roles, "Id", "Name", selectedRoles.Select(r => r.Id).ToList());

                    return View(userModel);

                case ButtonActionName.Delete:
                    return RedirectToAction("Delete", new { id = userModel.Id });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    Title = "Chỉnh sửa thông tin tài khoản " + userModel.FullName;
                    return View(userModel);

            }
        }

        //
        // GET: /Admin/User/Delete/5

        public ActionResult Delete(int id)
        {
            User user = userService.GetUserForUpdate(id);
            if (user == null)
            {
                ErrorNotification("Không tìm thấy tài khoản nào thỏa mãn");
                return RedirectToAction("Index");
            }
            Title = "Xóa tài khoản " + user.FullName;
            return View(user);
        }

        //
        // POST: /Admin/User/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Delete":
                    User user = userService.GetUserForUpdate(id);
                    if (user == null || user.Id == 1)
                    {
                        ErrorNotification("Không tìm thấy tài khoản nào thỏa mãn");
                        return RedirectToAction("Index");
                    }
                    //user
                    user.IsActive = false;
                    userService.UpdateUser(user);
                    SuccessNotification("Xóa tài khoản thành công");
                    return RedirectToAction("Index");
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
            }
        }

        //
        // GET: /Admin/Users/ChangePassword/5

        public ActionResult ChangePassword(int id)
        {
            User user = userService.GetUserForUpdate(id);
            if (user == null || (id == 1 && WorkContext.CurrentUserId != 1))
            {
                ErrorNotification("Không tìm thấy tài khoản nào thỏa mãn");
                return RedirectToAction("Index");
            }
            Title = "Đổi mật khẩu : " + user.UserName;

            var editModel = new ChangePasswordUserAdminModel
            {
                Id = id
            };
            ViewData["ToolbarTitle"] = Title;
            return View(editModel);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordUserAdminModel userModel)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case ButtonActionName.Save:
                case ButtonActionName.SaveAndContinueEdit:
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            User user = userService.GetUserForUpdate(userModel.Id);
                            if (user == null || (userModel.Id == 1 && WorkContext.CurrentUserId != 1))
                            {
                                ErrorNotification("Không tìm thấy tài khoản nào thỏa mãn");
                                return RedirectToAction("Index");
                            }
                            user.PasswordSalt = Util.RandomPasswordSalt();
                            user.Password = Util.ComputeMD5Hash(userModel.Password + user.PasswordSalt);
                            user.Modified = DateTime.Now;

                            userService.UpdateUser(user);

                            SuccessNotification("Đổi mật khẩu thành công");

                            if (command == ButtonActionName.SaveAndContinueEdit)
                            {
                                Title = "Chỉnh sửa thông tin tài khoản: " + user.UserName;
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        catch (Exception e)
                        {
                            Title = "Chỉnh sửa thông tin tài khoản";
                            ErrorNotification(e);
                        }
                    }

                    break;
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");

            }
            return View(userModel);
        }

        public ActionResult MyAccount()
        {
            User user = authenService.GetAuthenticatedUser();
            return View(user.ToEditModel());
        }
    }
}
