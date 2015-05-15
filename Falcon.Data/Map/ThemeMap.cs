using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Themes
    internal partial class ThemeMap : EntityTypeConfiguration<Theme>
    {
        public ThemeMap(string schema = "dbo")
        {
            ToTable(schema + ".Themes");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.ThemeType).HasColumnName("ThemeType").IsRequired().HasMaxLength(20);
            Property(x => x.ThemeName).HasColumnName("ThemeName").IsRequired().HasMaxLength(50);
            Property(x => x.IsStoreTheme).HasColumnName("IsStoreTheme").IsRequired();
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
