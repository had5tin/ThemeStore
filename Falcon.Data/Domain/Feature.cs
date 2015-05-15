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
    public partial class Feature : BaseEntity
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        //public virtual ICollection<ThemeFeature> ThemeFeatures { get; set; }
        public Feature()
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
