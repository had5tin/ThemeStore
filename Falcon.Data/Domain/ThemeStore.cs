using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Feature
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class ThemeStore : BaseEntity
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public string SeoUrl { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<double> Discount { get; set; }
        public string Image { get; set; }
        public string ImagePhone { get; set; }
        public string Demo { get; set; }
        public Nullable<int> Setting { get; set; }
        public string Description { get; set; }
        public string Support { get; set; }
        public string Documentation { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public ThemeStore()
        {
            CreatedOn = System.DateTime.Now;
            ModifiedOn = System.DateTime.Now;
            //LogDate = System.DateTime.Now;
            //ResetIpPermission = false;
            //this.ThemeFeatures = new List<ThemeFeature>();

            InitializePartial();
        }
        partial void InitializePartial();
    }

}
