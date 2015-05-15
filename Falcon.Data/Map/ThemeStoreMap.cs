using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Users
    internal partial class ThemeStoreMap : EntityTypeConfiguration<ThemeStore>
    {
        public ThemeStoreMap(string schema = "dbo")
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SeoUrl)
                .HasMaxLength(50);

            this.Property(t => t.Image)
                .HasMaxLength(50);

            this.Property(t => t.ImagePhone)
                .HasMaxLength(50);

            this.Property(t => t.Support)
                .HasMaxLength(500);

            this.Property(t => t.Documentation)
                .HasMaxLength(500);

            this.Property(t => t.CreatedBy)
                .HasMaxLength(50);

            this.Property(t => t.ModifiedBy)
                .HasMaxLength(50);

            // Table & Column Mappings
            //this.ToTable("ThemeStores");
            ToTable(schema + ".ThemeStores");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SeoUrl).HasColumnName("SeoUrl");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.ImagePhone).HasColumnName("ImagePhone");
            this.Property(t => t.Demo).HasColumnName("Demo");
            this.Property(t => t.Setting).HasColumnName("Setting");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Support).HasColumnName("Support");
            this.Property(t => t.Documentation).HasColumnName("Documentation");
            this.Property(t => t.CreatedBy).HasColumnName("CreatedBy");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedBy).HasColumnName("ModifiedBy");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");

            InitializePartial();
        }
        partial void InitializePartial();
    }

}
