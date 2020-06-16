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
    public class ApplicationConfiguration:EntityTypeConfiguration<Application>
    {
        public ApplicationConfiguration()
        {
            this.HasKey<Guid>(c => c.ApplicationId).Property(c => c.ApplicationId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();


            this.Property(c => c.Email).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Phone).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.Verified).HasColumnType("text").IsUnicode().IsRequired();
            this.Property(c => c.Salt).HasColumnType("text").IsUnicode().IsRequired();
            this.Property(c => c.Password).HasColumnType("text").IsUnicode().IsRequired();
            this.Property(c => c.DatetimeAdded).HasPrecision(0).IsRequired();



        }
    }
}