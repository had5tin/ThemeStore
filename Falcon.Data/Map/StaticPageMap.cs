using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Pages
    internal partial class StaticPageMap : EntityTypeConfiguration<StaticPage>
    {
        public StaticPageMap(string schema = "dbo")
        {
            ToTable(schema + ".Pages");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Title).HasColumnName("Title").IsRequired().HasMaxLength(255);
            Property(x => x.MetaKeyword).HasColumnName("MetaKeyword").IsOptional().HasMaxLength(500);
            Property(x => x.MetaDescription).HasColumnName("MetaDescription").IsOptional().HasMaxLength(500);
            Property(x => x.SeoUrl).HasColumnName("SeoUrl").IsOptional().HasMaxLength(255);
            Property(x => x.Content).HasColumnName("Content").IsRequired().HasMaxLength(1073741823);
            Property(x => x.Created).HasColumnName("Created").IsRequired();
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            Property(x => x.CreatedBy).HasColumnName("CreatedBy").IsOptional();
            Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsOptional();
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            Property(x => x.Layout).HasColumnName("Layout").IsOptional().HasMaxLength(50);
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
