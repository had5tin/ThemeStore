using System.Collections.Generic;
using System.Linq;
using Dapper;
using Falcon.Data.Domain;
using Falcon.Data.Item;
using Falcon.Data.Repository;

namespace Falcon.Services.Features
{
    public class FeatureService : BaseService, IFeatureService
    {
        private readonly IFeatureRepository _featureRepository;

        public FeatureService(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;
        }

        public IEnumerable<Feature> GetAll()
        {
            //var conn = GetOpenConnection();
            //IEnumerable<Feature> result;
            //result = conn.Query<Feature>("Select Id,UserName from Users");
            //conn.Close();
            //return result;
            return _featureRepository.Table.ToList();
        }

        public IEnumerable<Items> GetAllItems()
        {
            var list = _featureRepository.Table.ToList();
            return list.Select(feature => new Items() { Value = feature.Id.ToString(), Text = feature.Name }).ToList();
        }

    }
}
