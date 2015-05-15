using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Falcon.Mvc.Controllers;
using Falcon.Services.SystemSettings;
using Falcon.Data.Domain;
using Falcon.Admin.CoreModules.Models;
using Falcon.Common;
using System.Web.Script.Serialization;
using System.Collections;

namespace Falcon.Admin.CoreModules.Controllers
{
    public class SystemSettingController : AdminBaseController
    {
        private readonly ISystemSettingService _systemSettingService;

        public SystemSettingController(ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        //
        // GET: /Admin/SystemSetting/

        public ActionResult Index()
        {
            Title = "Quản trị SystemSetting";
            ViewData["ToolbarTitle"] = Title;

            IEnumerable<SystemSetting> systemSettings = _systemSettingService.GetAllSystemSetting();
            return View(systemSettings);
        }

        //
        // GET: /Admin/SystemSetting/Create

        public ActionResult Create()
        {
            Title = "Thêm mới SystemSetting";
            ViewData["ToolbarTitle"] = Title;
            SystemSettingModel model = new SystemSettingModel();
            var statuses = from SystemSettingcsEnum s in Enum.GetValues(typeof(SystemSettingcsEnum)) 
                           select new { ID = (int)s, Name = s.ToString() };
            model.SystemSettingType = new SelectList(statuses, "Id", "Name", 1);
            return View(model);
        }

        //
        // POST: /Admin/SystemSetting/Create

        [HttpPost]
        public ActionResult Create(SystemSettingModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SystemSetting systemSetting = model.ToEntity();
                    systemSetting.SettingKey = model.SettingKey;
                    systemSetting.Title = model.Title;
                    systemSetting.Description = model.Description;
                    systemSetting.Type = model.Type;
                    systemSetting.Value = model.Value;

                    if (model.Type == 1 || model.Type == 2)
                        systemSetting.Options = "";
                    else
                    {
                        //List<object> myObj = new List<object>();
                        //string[] options = model.Options.Trim().Split('|');
                        //foreach (var s in options)
                        //{
                        //    string[] strOption = s.Trim().Split('=');
                        //    myObj.Add(new { id = strOption[0], text = strOption[1] });
                        //}
                        //JavaScriptSerializer serializer = new JavaScriptSerializer();
                        //systemSetting.Options = serializer.Serialize(myObj);
                        systemSetting.Options = model.Options;
                    }
                        
                    systemSetting.IsRequired = model.IsRequired;

                    _systemSettingService.AddSystemSetting(systemSetting);
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
            Title = "Thêm mới SystemSetting";
            ViewData["ToolbarTitle"] = Title;
            var statuses = from SystemSettingcsEnum s in Enum.GetValues(typeof(SystemSettingcsEnum)) 
                           select new { ID = (int)s, Name = s.ToString() };
            model.SystemSettingType = new SelectList(statuses, "Id", "Name", 1);
            return View(model);
        }


        public ActionResult EditValue(string key)
        {
            SystemSetting systemSetting = _systemSettingService.GetForUpdate(key);
            if (systemSetting == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }
            Title = "Chỉnh sửa SystemSetting";
            ViewData["ToolbarTitle"] = Title;

            SystemSettingModel model = systemSetting.ToCreateModel();
            if (model.Type != 1 && model.Type != 2)
            {
                if (model.Options != null)
                {
                    List<object> myObj = new List<object>();
                    string[] options = model.Options.Trim().Split('|');
                    foreach (var s in options)
                    {
                        string[] strOption = s.Trim().Split('=');
                        myObj.Add(new { Id = strOption[0], Name = strOption[1] });
                    }
                    model.ListOptions = new SelectList(myObj, "Id", "Name", model.Value);
                }
            }
            if (model.Type == 3)
            {
                string[] arrValue = systemSetting.Value.Split(',');
                List<string> list = new List<string>();
                for (int i = 0; i < arrValue.Length; i++)
                {
                    list.Add(arrValue[i]);
                }
                model.ListSelected = list;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditValue(SystemSettingModel model)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            SystemSetting systemSetting = _systemSettingService.GetForUpdate(model.SettingKey);
                            if (systemSetting == null)
                            {
                                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                                return RedirectToAction("Index");
                            }
                            if (systemSetting.Type == 3)
                            {
                                string value = "";
                                foreach (var item in model.ListSelected)
                                {
                                    value += item + ",";
                                }
                                systemSetting.Value = value.Trim(',');
                            }
                            else
                                systemSetting.Value = model.Value;
                            _systemSettingService.UpdateSystemSetting(systemSetting);

                            SuccessNotification("Cập nhật SystemSetting thành công");

                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa SystemSetting ";
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }

                        }
                        catch (Exception e)
                        {
                            Title = "Chỉnh sửa SystemSetting";
                            ErrorNotification(e);
                        }
                    }

                    break;
                case "Delete":
                    return RedirectToAction("Delete", new { key = model.SettingKey });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");

            }

            ViewData["ToolbarTitle"] = Title;
            var statuses = from SystemSettingcsEnum s in Enum.GetValues(typeof(SystemSettingcsEnum))
                           select new { ID = (int)s, Name = s.ToString() };
            model.SystemSettingType = new SelectList(statuses, "Id", "Name", 1);
            return View(model);
        }

        //
        // GET: /Admin/SystemSetting/Edit/5

        public ActionResult Edit(string key)
        {
            SystemSetting systemSetting = _systemSettingService.GetSystemSetting(key);
            if (systemSetting == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }
            Title = "Chỉnh sửa SystemSetting";
            ViewData["ToolbarTitle"] = Title;

            SystemSettingModel model = systemSetting.ToCreateModel();
            if (model.Type != 1 && model.Type != 2)
            {
                if (model.Options != null)
                {
                    List<object> myObj = new List<object>();
                    string[] options = model.Options.Trim().Split('|');
                    foreach (var s in options)
                    {
                        string[] strOption = s.Trim().Split('=');
                        myObj.Add(new { Id = strOption[0], Name = strOption[1] });
                    }
                    model.ListOptions = new SelectList(myObj, "Id", "Name", model.Value);
                }
            }
            if (model.Type == 3)
            {
                string[] arrValue = systemSetting.Value.Split(',');
                List<string> list = new List<string>();
                for (int i = 0; i < arrValue.Length; i++)
                {
                    list.Add(arrValue[i]);
                }
                model.ListSelected = list;
            }
            var statuses = from SystemSettingcsEnum s in Enum.GetValues(typeof(SystemSettingcsEnum))
                           select new { ID = (int)s, Name = s.ToString() };

            model.SystemSettingType = new SelectList(statuses, "Id", "Name", model.Type);
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //List<SelectListItem> dataObj = serializer.Deserialize<List<SelectListItem>>(model.Options);
            //model.ListOptions = new SelectList(dataObj, "Value", "Text", model.Value);
            return View(model);
        }

        //
        // POST: /Admin/SystemSetting/Edit/5

        [HttpPost]
        public ActionResult Edit(SystemSettingModel model)
        {
            SystemSetting systemSetting = _systemSettingService.GetSystemSetting(model.SettingKey);
            if (systemSetting == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }

            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Save":
                case "SaveAndContinueEdit":
                    if (ModelState.IsValid)
                    {
                        try
                        {                            
                            systemSetting.SettingKey = model.SettingKey;
                            systemSetting.Title = model.Title;
                            systemSetting.Description = model.Description;
                            //if (systemSetting.Type == 3)
                            //{
                            //    string value = "";
                            //    foreach (var item in model.ListSelected)
                            //    {
                            //        value += item + ",";
                            //    }
                            //    systemSetting.Value = value.Trim(',');
                            //}
                            //else
                            //    systemSetting.Value = model.Value;
                            systemSetting.Type = model.Type;
                            if (model.Type == 1 || model.Type==2)
                                systemSetting.Options = "";
                            else
                                systemSetting.Options = model.Options;
                            systemSetting.IsRequired = model.IsRequired;

                            _systemSettingService.UpdateSystemSetting(systemSetting);

                            SuccessNotification("Cập nhật SystemSetting thành công");

                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa SystemSetting ";
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }

                        }
                        catch (Exception e)
                        {
                            Title = "Chỉnh sửa SystemSetting ";
                            ErrorNotification(e);
                        }
                    }

                    break;
                case "Delete":
                    return RedirectToAction("Delete", new { key = model.SettingKey });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");

            }

            if (model.Type != 1 && model.Type != 2)
            {
                if (model.Options != null)
                {
                    List<object> myObj = new List<object>();
                    string[] options = model.Options.Trim().Split('|');
                    foreach (var s in options)
                    {
                        string[] strOption = s.Trim().Split('=');
                        myObj.Add(new { Id = strOption[0], Name = strOption[1] });
                    }
                    model.ListOptions = new SelectList(myObj, "Id", "Name", model.Value);
                }
            }
            if (model.Type == 3)
            {
                string[] arrValue = systemSetting.Value.Split(',');
                List<string> list = new List<string>();
                for (int i = 0; i < arrValue.Length; i++)
                {
                    list.Add(arrValue[i]);
                }
                model.ListSelected = list;
            }
            var statuses = from SystemSettingcsEnum s in Enum.GetValues(typeof(SystemSettingcsEnum))
                           select new { ID = (int)s, Name = s.ToString() };

            model.SystemSettingType = new SelectList(statuses, "Id", "Name", model.Type);

            ViewData["ToolbarTitle"] = Title;
            return View(model);
        }

        //
        // POST: /Admin/SystemSetting/Delete/5

        [HttpPost]
        public ActionResult Delete(string key)
        {
            SystemSetting systemSetting = _systemSettingService.GetSystemSetting(key);
            if (systemSetting == null)
            {
                ErrorNotification("Không tìm thấy trang nào thỏa mãn");
                return RedirectToAction("Index");
            }
            _systemSettingService.RemoveSystemSetting(systemSetting);
            SuccessNotification("Xóa SystemSetting thành công");
            return RedirectToAction("Index");
        } 
    }
}
