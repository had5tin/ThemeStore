using System.Collections.Generic;
using System.Linq;
using Falcon.Data.Domain;
using Falcon.Data.Repository;
using Falcon.Services.ThemeFeatures;

namespace Falcon.Services.ThemeColletions
{
    public class ThemeColletionService : BaseService, IThemeColletionService
    {
        private readonly IThemeColletionRepository _themeColletionRepository;

        public ThemeColletionService(IThemeColletionRepository themeColletionRepository)
        {
            _themeColletionRepository = themeColletionRepository;
        }

        public IEnumerable<ThemeColletion> GetAll()
        {
            //var conn = GetOpenConnection();
            //IEnumerable<Feature> result;
            //result = conn.Query<Feature>("Select Id,UserName from Users");
            //conn.Close();
            //return result;
            return _themeColletionRepository.Table.ToList();
        }

        public IEnumerable<ThemeColletion> GetByColletionId(int colletionId)
        {
            return _themeColletionRepository.Table.Where(f => f.CollectionId == colletionId);
        }

        public IEnumerable<ThemeColletion> GetByThemeId(int themeId)
        {
            return _themeColletionRepository.Table.Where(f => f.ThemeId == themeId);
        }

    }
}
