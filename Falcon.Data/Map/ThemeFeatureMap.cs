using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Users
    internal partial class ThemeFeatureMap : EntityTypeConfiguration<ThemeFeature>
    {
        public ThemeFeatureMap(string schema = "dbo")
        {
            // Primary Key
            this.HasKey(t => new { t.ThemeId, t.FeatureId });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ThemeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FeatureId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            //this.ToTable("ThemeFeatures");
            ToTable(schema + ".ThemeFeatures");

            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ThemeId).HasColumnName("ThemeId");
            this.Property(t => t.FeatureId).HasColumnName("FeatureId");

            //// Relationships
            //this.HasRequired(t => t.Feature)
            //    .WithMany(t => t.ThemeFeatures)
            //    .HasForeignKey(d => d.FeatureId);
            //this.HasRequired(t => t.ThemesU)
            //    .WithMany(t => t.ThemeFeatures)
            //    .HasForeignKey(d => d.ThemeId);

            InitializePartial();
        }
        partial void InitializePartial();
    }

}
