using System.Collections.Generic;
using System.Linq;
using Falcon.Data.Domain;
using Falcon.Data.Item;
using Falcon.Data.Repository;
using Falcon.Services.ThemeColletions;

namespace Falcon.Services.Collections
{
    public class CollectionService : BaseService, ICollectionService
    {
        private readonly ICollectionRepository _collectionRepository;
        private readonly IThemeColletionService _themeColletionService;
        public CollectionService(ICollectionRepository collectionRepository, IThemeColletionService themeColletionService)
        {
            _collectionRepository = collectionRepository;
            _themeColletionService = themeColletionService;
        }


        public IEnumerable<Collection> GetAll()
        {
            return _collectionRepository.Table.ToList();
        }

        public IEnumerable<Items> GetAllItems()
        {
            var list = _collectionRepository.Table.ToList();
            return list.Select(collection => new Items() { Value = collection.Id.ToString(), Text = collection.Name }).ToList();
        }

        public void Add(Collection item)
        {
            _collectionRepository.Add(item);
        }

        public void Del(int id)
        {
            var item = GetById(id);
            _collectionRepository.Remove(item);
        }

        public void Edit(Collection item)
        {
            //var collection = GetById(item.Id);
            //collection.
            var col = _collectionRepository.Table.FirstOrDefault(m => m.Id == item.Id);
            if (col != null)
            {
                col.Name = item.Name;
                col.Description = item.Description;
                col.ModifiedOn = item.ModifiedOn;
            }

            _collectionRepository.SubmitChanges();
        }

        public Collection GetById(int id)
        {
            return _collectionRepository.Table.FirstOrDefault(m => m.Id == id);
        }

        public List<string> GetByThemeId(int themeId)
        {
            var list = _themeColletionService.GetByThemeId(themeId).ToList();
            var listCol = list.Select(m => GetById(m.CollectionId)).Where(item => item != null).ToList();
            return listCol.Select(col => col.Id.ToString()).ToList();
        }
    }
}
