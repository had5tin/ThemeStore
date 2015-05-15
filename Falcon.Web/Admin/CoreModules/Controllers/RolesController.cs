using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Mvc.Controllers;
using Falcon.Security;
using Falcon.Data.Domain;
using Falcon.Admin.CoreModules.Models;
using Falcon.UI.Html.Admin;

namespace Falcon.Admin.CoreModules.Controllers
{
    public class RolesController : AdminBaseController
    {
        private readonly IAuthorizationService _authService;

        public RolesController(IAuthorizationService authService)
        {
            _authService = authService;
        }

        //
        // GET: /AdminRole/

        public ActionResult Index()
        {
            Title = "Danh sách vai trò";
            IEnumerable<Role> roles = _authService.GetAllRoles();
            return View(roles);
        }

        public ActionResult Create()
        {
            Title = "Thêm mới vai trò";
            RoleModel model = new RoleModel();
            //model.RoleList = new SelectList(_authService.GetAllRoles(), "Id", "Name", model.ParentId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Role role = model.ToEntity();

                    _authService.AddRole(role);
                    SuccessNotification(String.Format("Thêm mới vai trò {0} thành công", role.Name));
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ErrorNotification(e.Message);
                }
            }
            else
            {
                AddModelStateErrors();
            }
            //model.RoleList = new SelectList(_authService.GetAllRoles(), "Id", "Name", model.ParentId);

            Title = "Thêm mới vai trò";
            return View(model);
        }
        
        public ActionResult Edit(int id)
        {
            Role role = _authService.GetRole(id);
            if (role == null)
            {
                ErrorNotification("Tham số truyền vào không chính xác");
                return RedirectToAction("Index");
            }

            RoleModel model = role.ToModel();
            //model.RoleList = new SelectList(_authService.GetAllRoles(), "Id", "Name", model.ParentId);

            Title = "Chỉnh sửa vai trò: " + role.Name;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Role role = model.ToEntity();

                    _authService.UpdateRole(role);
                    SuccessNotification(String.Format("Cập nhật thông tin vai trò {0} thành công", role.Name));
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ErrorNotification(e.Message);
                }
            }
            else
            {
                AddModelStateErrors();
            }
            //model.RoleList = new SelectList(_authService.GetAllRoles(), "Id", "Name", model.ParentId);
            Title = "Chỉnh sửa vai trò: " + model.Name;
            return View(model);
        }

        public ActionResult ListUsers(int id)
        {
            IEnumerable<User> users = _authService.GetAllUsersByRole(id);
            return View(users);
        }

        public ActionResult EditPermission(int id)
        {
            Role role = _authService.GetRole(id);
            if (role == null)
            {
                ErrorNotification("Tham số truyền vào không chính xác");
                return RedirectToAction("Index");
            }

            Title = "Phân quyền vai trò: " + role.Name;
            return View(role);
        }

        [HttpPost]
        public ActionResult EditPermission(int id, FormCollection collection)
        {
            Role role = _authService.GetRole(id);
            if (role == null)
            {
                ErrorNotification("Tham số truyền vào không chính xác");
                return RedirectToAction("Index");
            }

            List<Permission> permissions = new List<Permission>();
            foreach (var item in collection.AllKeys)
            {
                if (item.StartsWith("check_"))
                {
                    string[] tmp = item.Split('_');
                    //valid item like: check_Users_Admin_Edit
                    if (tmp.Length == 4)
                    {
                        string module = tmp[1];
                        string controller = tmp[2];
                        string action = tmp[3];

                        Permission permission = new Permission()
                        {
                            ResourceName = module + "/" + controller,
                            RoleId = id,
                            Privilege = action,
                            IsAllowed = true
                        };

                        permissions.Add(permission);
                    }
                }
            }
            try
            {
                _authService.UpdateRolePermission(id, permissions);
                SuccessNotification("Cập nhật quyền cho vai trò " + role.Name + " thành công");

                string command = Request.Form["submit"].ToString();
                if (command == ButtonActionName.SaveAndContinueEdit)
                {
                    return View(role);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex) 
            {
                return View(role);
            }            
        }
        
        public JsonResult GetRoleTreeData(int id)
        {
            List<JsTreeModel> result = new List<JsTreeModel>();

            ModuleProvider modules = new ModuleProvider(LoadModuleType.FromAssembly);
            IEnumerable<ModuleConfiguration> moduleConfigs = modules.ModuleConfigurations.Where(module => module.Controllers.Count > 0);
            IEnumerable<Permission_GetByRoleIdResult> lstPermision = _authService.GetPermissionsByRoleId(id);

            foreach(var module in moduleConfigs)
            {
                JsTreeModel mdlTree = new JsTreeModel();
                
                if (!string.IsNullOrEmpty(module.Description))
                {
                    mdlTree.data = module.Name + " - " + module.Description;
                }
                else
                {
                    mdlTree.data = module.Name;
                }

                mdlTree.attr.id = module.Name;

                bool mdlChecked = true;

                foreach (var ctl in module.Controllers)
                {
                    JsTreeModel ctlTree = new JsTreeModel();
                    ctlTree.data = ctl.Name;

                    ctlTree.attr.id = module.Name + "_" + ctl.Name;

                    bool ctlChecked = true;

                    foreach (var act in ctl.Actions)
                    {
                        JsTreeModel actTree = new JsTreeModel();
                        actTree.data = act;

                        Permission_GetByRoleIdResult permission = lstPermision.Where(p => (p.ResourceName == module.Name + "/" + ctl.Name) && p.Privilege == act).SingleOrDefault();

                        actTree.attr.id = module.Name + "_" + ctl.Name + "_" + act;
                        if (permission != null)
                        {                            
                            actTree.attr.selected = permission.IsAllowed;
                            if (!permission.IsAllowed)
                            {
                                ctlChecked = false;
                            }
                        }
                        else
                        {
                            actTree.attr.selected = false;
                            ctlChecked = false;
                        }

                        ctlTree.children.Add(actTree);
                    }

                    ctlTree.attr.selected = ctlChecked;

                    if (!ctlChecked)
                    {
                        mdlChecked = false;
                    }

                    mdlTree.children.Add(ctlTree);
                }

                mdlTree.attr.selected = mdlChecked;

                result.Add(mdlTree);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GenerateModuleConfigFile(int id)
        {
            PermissionExplorer permissionExp = new PermissionExplorer();
            List<PermissionInfo> permissions = permissionExp.GetAllControllers();

            return View(permissions);
        }
    }
}
