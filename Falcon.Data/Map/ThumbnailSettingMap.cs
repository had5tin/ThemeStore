using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // ThumbnailSettings
    internal partial class ThumbnailSettingMap : EntityTypeConfiguration<ThumbnailSetting>
    {
        public ThumbnailSettingMap(string schema = "dbo")
        {
            ToTable(schema + ".ThumbnailSettings");
            HasKey(x => x.Id);

            Property(x => x.ThumbSize).HasColumnName("ThumbSize").IsRequired().HasMaxLength(20);
            Property(x => x.Width).HasColumnName("Width").IsRequired();
            Property(x => x.Height).HasColumnName("Height").IsRequired();
            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
