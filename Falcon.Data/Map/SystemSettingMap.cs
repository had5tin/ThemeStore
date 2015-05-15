using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // SystemSettings
    internal partial class SystemSettingMap : EntityTypeConfiguration<SystemSetting>
    {
        public SystemSettingMap(string schema = "dbo")
        {
            ToTable(schema + ".SystemSettings");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.SettingKey).HasColumnName("setting_key").IsRequired().HasMaxLength(50);
            Property(x => x.Title).HasColumnName("Title").IsOptional().HasMaxLength(100);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.Type).HasColumnName("Type").IsOptional();
            Property(x => x.Value).HasColumnName("Value").IsOptional();
            Property(x => x.Options).HasColumnName("Options").IsOptional().HasMaxLength(255);
            Property(x => x.IsRequired).HasColumnName("IsRequired").IsOptional();
            Property(x => x.Created).HasColumnName("Created").IsRequired();
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsRequired();
            Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
