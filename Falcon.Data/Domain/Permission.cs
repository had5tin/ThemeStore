using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Permissions
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class Permission : BaseEntity
    {
        public int RoleId { get; set; } // RoleId
        public int ResourceId { get; set; } // ResourceId
        public string Privilege { get; set; } // Privilege
        public bool IsAllowed { get; set; } // IsAllowed
        public override int Id { get; set; } // Id (Primary key)
        public DateTime Modified { get; set; } // Modified

        public Permission()
        {
            Modified = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
