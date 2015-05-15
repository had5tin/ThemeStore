using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data;
using Falcon.Data.Domain;
using Falcon.Data.Repository;

namespace Falcon.Services.Pages
{
    public interface IPageService : IService
    {
        StaticPage GetPageById(int pageId);
        StaticPage GetPageBySeoUrl(string seoUrl);
        int AddPage(StaticPage page);
        void UpdatePage(StaticPage page);
        void RemovePage(StaticPage page);

        IEnumerable<StaticPage> GetAll();
    }
}
