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
using Falcon.Services.Pages;

namespace Falcon.Modules.Contents.Controllers
{
    public class PagesController : BaseController
    {
        private readonly IPageService _pageService;

        public PagesController(IPageService pageService)
        {
            this._pageService = pageService;
        }
        
        public ActionResult Details(string title)
        {
            StaticPage page = _pageService.GetPageBySeoUrl(title);
            if (page == null)
            {
                return ShowErrorPage("Xin lỗi bạn, hệ thống không tìm thấy trang: " + title);
            }

            Title = page.Title;
            MetaDescription = page.MetaDescription;
            MetaKeyword = page.MetaKeyword;
            LayoutName = page.Layout;

            return View(page);
        }
        
        public ActionResult Error(string path)
        {
            return View();
            //return ShowErrorPage("Xin lỗi bạn, hệ thống không tìm thấy trang: " + path);
        }
    }
}
