using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data.Domain;
using Falcon.Data.Repository;
using Dapper;

namespace Falcon.Services.Pages
{
    public class PageService : BaseService, IPageService
    {
        private readonly IPageRepository _pageRepository;

        public PageService(IPageRepository pageRepository)
        {
            this._pageRepository = pageRepository;
        }
      
        public StaticPage GetPageById(int pageId)
        {
            return _pageRepository.Table.FirstOrDefault(p => p.Id == pageId);
        }

        public StaticPage GetPageBySeoUrl(string seoUrl)
        {
            return _pageRepository.Table.FirstOrDefault(p => p.SeoUrl == seoUrl);
        }

        public int AddPage(StaticPage page)
        {
            _pageRepository.Add(page);
            return page.Id;
        }

        public void UpdatePage(StaticPage page)
        {
            _pageRepository.SubmitChanges();
        }

        public void RemovePage(StaticPage page)
        {
            _pageRepository.Remove(page);
        }


        public IEnumerable<StaticPage> GetAll()
        {
            var conn = GetOpenConnection();
            IEnumerable<StaticPage> result;
            result = conn.Query<StaticPage>("Select Id, Title, SeoUrl, Created, Modified, IsActive, Layout from Pages");
            conn.Close();
            return result;
            //return _pageRepository.Table.ToList<StaticPage>();
        }
    }
}
