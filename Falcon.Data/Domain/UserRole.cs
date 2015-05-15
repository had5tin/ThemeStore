using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // UserRole
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class UserRole : BaseEntity
    {
        public int UserId { get; set; } // UserId
        public int RoleId { get; set; } // RoleId
        public override int Id { get; set; } // Id (Primary key)
        public DateTime Modified { get; set; } // Modified

        public UserRole()
        {
            Modified = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
