using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Permissions
    internal partial class PermissionMap : EntityTypeConfiguration<Permission>
    {
        public PermissionMap(string schema = "dbo")
        {
            ToTable(schema + ".Permissions");
            HasKey(x => x.Id);

            Property(x => x.RoleId).HasColumnName("RoleId").IsRequired();
            Property(x => x.ResourceId).HasColumnName("ResourceId").IsRequired();
            Property(x => x.Privilege).HasColumnName("Privilege").IsRequired().HasMaxLength(64);
            Property(x => x.IsAllowed).HasColumnName("IsAllowed").IsRequired();
            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
