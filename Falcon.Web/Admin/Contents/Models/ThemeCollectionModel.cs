using Falcon.Data.Item;
using System.Collections.Generic;

namespace Falcon.Admin.Modules.Contents.Models
{
    public class ThemeCollectionModel
    {
        public int Id { get; set; }
        public List<string> CollectionSelect { get; set; }
        public List<Items> CollectionList { get; set; }

    }
}