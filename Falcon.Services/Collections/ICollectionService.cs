using System.Collections.Generic;
using Falcon.Data.Domain;
using Falcon.Data.Item;

namespace Falcon.Services.Collections
{
    public interface ICollectionService : IService
    {
        IEnumerable<Collection> GetAll();
        IEnumerable<Items> GetAllItems();
        void Add(Collection item);
        void Del(int id);
        void Edit(Collection item);
        Collection GetById(int id);

        List<string> GetByThemeId(int themeId);
    }
}
