using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // Themes
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class Theme : BaseEntity
    {
        public override int Id { get; set; } // Id (Primary key)
        public string ThemeType { get; set; } // ThemeType
        public string ThemeName { get; set; } // ThemeName
        public bool IsStoreTheme { get; set; } // IsStoreTheme
        public DateTime Modified { get; set; } // Modified

        public Theme()
        {
            IsStoreTheme = false;
            Modified = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
