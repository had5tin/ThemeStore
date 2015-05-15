using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using ProtoBuf;

namespace Falcon.Data.Domain
{
    // ThumbnailSettings
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic, ImplicitFirstTag = 1)]
    public partial class ThumbnailSetting : BaseEntity
    {
        public string ThumbSize { get; set; } // ThumbSize
        public int Width { get; set; } // Width
        public int Height { get; set; } // Height
        public override int Id { get; set; } // Id (Primary key)
        public DateTime Modified { get; set; } // Modified

        public ThumbnailSetting()
        {
            Modified = System.DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
