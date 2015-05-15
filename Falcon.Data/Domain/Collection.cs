using System;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Collection
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class Collection : BaseEntity
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }

        public Collection()
        {
            CreatedOn = System.DateTime.Now;
            ModifiedOn = System.DateTime.Now;
            //LogDate = System.DateTime.Now;
            //ResetIpPermission = false;
            InitializePartial();
        }
        partial void InitializePartial();
    }
}
