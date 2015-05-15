using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Resources
    internal partial class ResourceMap : EntityTypeConfiguration<Resource>
    {
        public ResourceMap(string schema = "dbo")
        {
            ToTable(schema + ".Resources");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(64);
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.ParentId).HasColumnName("ParentId").IsOptional();
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
