using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // SystemSettings
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class SystemSetting : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string SettingKey { get; set; } // setting_key
        public string Title { get; set; } // Title
        public string Description { get; set; } // Description
        public int? Type { get; set; } // Type
        public string Value { get; set; } // Value
        public string Options { get; set; } // Options
        public bool? IsRequired { get; set; } // IsRequired
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public int CreatedBy { get; set; } // CreatedBy
        public int ModifiedBy { get; set; } // ModifiedBy

        public SystemSetting()
        {
            Created = System.DateTime.Now;
            Modified = System.DateTime.Now;
            CreatedBy = 1;
            ModifiedBy = 1;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
