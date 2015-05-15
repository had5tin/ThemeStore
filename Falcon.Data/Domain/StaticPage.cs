using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Pages
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class StaticPage : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string Title { get; set; } // Title
        public string MetaKeyword { get; set; } // MetaKeyword
        public string MetaDescription { get; set; } // MetaDescription
        public string SeoUrl { get; set; } // SeoUrl
        public string Content { get; set; } // Content
        public DateTime Created { get; set; } // Created
        public DateTime Modified { get; set; } // Modified
        public int? CreatedBy { get; set; } // CreatedBy
        public int? ModifiedBy { get; set; } // ModifiedBy
        public bool IsActive { get; set; } // IsActive
        public string Layout { get; set; } // Layout

        public StaticPage()
        {
            Created = System.DateTime.Now;
            IsActive = true;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
