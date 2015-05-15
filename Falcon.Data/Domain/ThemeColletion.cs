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
    public partial class ThemeColletion : BaseEntity
    {
        public override int Id { get; set; }
        public int ThemeId { get; set; }
        public int CollectionId { get; set; }
        public ThemeColletion()
        {
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
