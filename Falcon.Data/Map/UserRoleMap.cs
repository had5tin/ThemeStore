using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // UserRole
    internal partial class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap(string schema = "dbo")
        {
            ToTable(schema + ".UserRole");
            HasKey(x => x.Id);

            Property(x => x.UserId).HasColumnName("UserId").IsRequired();
            Property(x => x.RoleId).HasColumnName("RoleId").IsRequired();
            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
