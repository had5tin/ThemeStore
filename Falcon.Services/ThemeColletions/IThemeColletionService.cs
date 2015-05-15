using System.Collections.Generic;
using Falcon.Data.Domain;

namespace Falcon.Services.ThemeColletions
{
    public interface IThemeColletionService : IService
    {
        IEnumerable<ThemeColletion> GetAll();
        IEnumerable<ThemeColletion> GetByColletionId(int colletionId);
        IEnumerable<ThemeColletion> GetByThemeId(int themeId);
    }
}
