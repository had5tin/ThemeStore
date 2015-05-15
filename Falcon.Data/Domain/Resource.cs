using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Resources
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class Resource : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name
        public string Description { get; set; } // Description
        public int? ParentId { get; set; } // ParentId
        public DateTime Modified { get; set; } // Modified

        public Resource()
        {
            Modified = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
