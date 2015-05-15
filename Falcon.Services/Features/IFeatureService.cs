using Falcon.Data.Domain;
using Falcon.Data.Item;
using System.Collections.Generic;

namespace Falcon.Services.Features
{
    public interface IFeatureService:IService
    {
        IEnumerable<Feature> GetAll();
        IEnumerable<Items> GetAllItems();

    }
}
