using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Falcon.Data.Domain;
using AutoMapper;

namespace Falcon.Admin.Modules.Contents.Models
{
    public static class MappingExtensions
    {
        #region StaticPageModel
        public static StaticPageModel ToModel(this StaticPage entity)
        {
            return Mapper.Map<StaticPage, StaticPageModel>(entity);
        }

        public static StaticPage ToEntity(this StaticPageModel model)
        {
            return Mapper.Map<StaticPageModel, StaticPage>(model);
        }
        #endregion        
        
    }
}