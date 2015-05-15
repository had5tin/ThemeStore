using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using Falcon.Mvc.Controllers;
using Falcon.Security;

using Falcon.Services.Pages;
using Falcon.Data.Domain;
using Falcon.Admin.Modules.Contents.Models;
using Falcon.UI.Html.Admin;
//using StackExchange.Profiling;

namespace Falcon.Admin.Modules.Contents.Controllers
{    
    public class PagesController : AdminBaseController
    {
        private readonly IPageService _pageService; 
        private readonly IAuthenticationService _authenService;

        public PagesController(IPageService pageService, IAuthenticationService authenService)
        {
            _pageService = pageService;
            _authenService = authenService;
        }
        //
        // GET: /Admin/Pages/

        public ActionResult Index()
        {
            Title = "Quản trị trang tĩnh";
            ViewData["ToolbarTitle"] = Title;
            IEnumerable<StaticPage> pages;
            //using (Profiler.Step("GetAllPages"))
            //{                
                pages = _pageService.GetAll();
            //}
            
            return View(pages);
        }

        //
        // GET: /Admin/Pages/Details/5

        public ActionResult Details(int id)
        {
            Title = "Chi tiết trang tĩnh";
            ViewData["ToolbarTitle"] = Title;
            return View();
        }

        //
        // GET: /Admin/Pages/Create

        public ActionResult Create()
        {
            Title = "Thêm mới trang tĩnh";
            ViewData["ToolbarTitle"] = Title;
            return View();
        } 

        //
        // POST: /Admin/Pages/Create

        [HttpPost]
        public ActionResult Create(StaticPageModel pageModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = _authenService.GetAuthenticatedUser();

                    StaticPage page = pageModel.ToEntity();
                    page.Created = DateTime.Now;
                    page.Modified = page.Created;
                    page.CreatedBy = user.Id;
                    page.ModifiedBy = page.CreatedBy;

                    _pageService.AddPage(page);

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ErrorNotification(e.ToString());
                }
            }
            else
            {
                AddModelStateErrors();                
            }
            Title = "Thêm mới trang tĩnh";
            ViewData["ToolbarTitle"] = Title;
            return View();
            
        }
        
        //
        // GET: /Admin/Pages/Edit/5
 
        public ActionResult Edit(int id)
        {            
            StaticPage page = _pageService.GetPageById(id);
            if (page == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }
            Title = "Chỉnh sửa thông tin trang tĩnh: " + page.Title;
            ViewData["ToolbarTitle"] = Title;
            return View(page.ToModel());
        }

        //
        // POST: /Admin/Pages/Edit/5

        [HttpPost]
        public ActionResult Edit(StaticPageModel pageModel)
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
                            StaticPage page = _pageService.GetPageById(pageModel.Id);
                            if (page == null)
                            {
                                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                                return RedirectToAction("Index");
                            }
                            User user = _authenService.GetAuthenticatedUser();

                            page.Title = pageModel.Title;
                            page.SeoUrl = pageModel.SeoUrl;
                            page.MetaDescription = pageModel.MetaDescription;
                            page.MetaKeyword = pageModel.MetaKeyword;
                            page.IsActive = pageModel.IsActive;
                            page.Content = pageModel.Content;
                            page.Modified = DateTime.Now;
                            page.ModifiedBy = user.Id;

                            _pageService.UpdatePage(page);

                            SuccessNotification("Cập nhật thông tin trang tĩnh thành công");

                            if (command == "SaveAndContinueEdit")
                            {
                                Title = "Chỉnh sửa thông tin trang tĩnh: " + page.Title;
                                //return View(pageModel);
                            }
                            else
                            {
                                return RedirectToAction("Index");
                            }
                        }
                        catch (Exception e)
                        {
                            Title = "Chỉnh sửa thông tin trang tĩnh";
                            ErrorNotification(e.ToString());
                        }
                    }
                    else
                    {
                        AddModelStateErrors();
                    }

                    break;
                case "Delete":
                    return RedirectToAction("Delete", new { id = pageModel.Id });

                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
                    
            }
            
            ViewData["ToolbarTitle"] = Title;
            return View(pageModel);
        }

        //
        // GET: /Admin/Pages/Delete/5
 
        public ActionResult Delete(int id)
        {
            StaticPage page = _pageService.GetPageById(id);
            if (page == null)
            {
                ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                return RedirectToAction("Index");
            }
            
            return View(page);
        }

        //
        // POST: /Admin/Pages/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string command = Request.Form["submit"].ToString();
            switch (command)
            {
                case "Delete":
                    StaticPage page = _pageService.GetPageById(id);
                    if (page == null)
                    {
                        ErrorNotification("Không tìm thấy trang tĩnh nào thỏa mãn");
                        return RedirectToAction("Index");
                    }
                    //page
                    _pageService.UpdatePage(page);
                    SuccessNotification("Xóa trang tĩnh trang tĩnh thành công");
                    return RedirectToAction("Index");
                default:
                    ErrorNotification("Không rõ phương thức submit dữ liệu");
                    return RedirectToAction("Index");
            }            
        }      
    }
}
