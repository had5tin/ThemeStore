using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Users
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class User : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string UserName { get; set; } // UserName
        public string Password { get; set; } // Password
        public string PasswordSalt { get; set; } // PasswordSalt
        public string Email { get; set; } // Email
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public string FullName { get; set; } // FullName
        public bool IsActive { get; set; } // IsActive
        public string Address { get; set; } // Address
        public DateTime? LogDate { get; set; } // LogDate
        public int? LogNum { get; set; } // LogNum
        public string Description { get; set; } // Description
        public bool ResetIpPermission { get; set; } // ResetIpPermission

        public User()
        {
            Created = System.DateTime.Now;
            Modified = System.DateTime.Now;
            LogDate = System.DateTime.Now;
            ResetIpPermission = false;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
