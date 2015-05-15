using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data;
using Falcon.Data.Domain;

namespace Falcon.Data.Map
{
    // Users
    internal partial class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap(string schema = "dbo")
        {
            ToTable(schema + ".Users");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(50);
            Property(x => x.Password).HasColumnName("Password").IsRequired().HasMaxLength(64);
            Property(x => x.PasswordSalt).HasColumnName("PasswordSalt").IsRequired().HasMaxLength(10);
            Property(x => x.Email).HasColumnName("Email").IsRequired().HasMaxLength(128);
            Property(x => x.Created).HasColumnName("Created").IsRequired();
            Property(x => x.Modified).HasColumnName("Modified").IsRequired();
            Property(x => x.FullName).HasColumnName("FullName").IsRequired().HasMaxLength(100);
            Property(x => x.IsActive).HasColumnName("IsActive").IsRequired();
            Property(x => x.Address).HasColumnName("Address").IsOptional().HasMaxLength(255);
            Property(x => x.LogDate).HasColumnName("LogDate").IsOptional();
            Property(x => x.LogNum).HasColumnName("LogNum").IsOptional();
            Property(x => x.Description).HasColumnName("Description").IsOptional().HasMaxLength(255);
            Property(x => x.ResetIpPermission).HasColumnName("ResetIpPermission").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

}
