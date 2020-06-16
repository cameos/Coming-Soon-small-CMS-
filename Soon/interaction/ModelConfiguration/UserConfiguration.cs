using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Soon.interaction.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Soon.interaction.ModelConfiguration
{
    public class UserConfiguration:EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            this.HasKey<Guid>(c => c.UserId).Property(c => c.UserId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();


            this.Property(c => c.Name).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Surname).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Username).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();

            this.HasRequired<Application>(c => c.Application).WithMany(c => c.Users).HasForeignKey<Guid>(c => c.ApplicationId).WillCascadeOnDelete(false);

        }
    }
}