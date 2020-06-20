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
    public class ArticleConfiguration:EntityTypeConfiguration<Articles>
    {
        public ArticleConfiguration()
        {
            this.HasKey<Guid>(c => c.ArticlesId).Property(c => c.ArticlesId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();


            this.Property(c => c.ArticleTitle).HasColumnType("nvarchar").HasMaxLength(300).IsFixedLength().IsRequired();
            this.Property(c => c.Body).HasColumnType("text").IsRequired();
            this.Property(c => c.Image).HasColumnType("image").IsRequired();
            this.Property(c => c.ImimeType).HasColumnType("nvarchar").HasMaxLength(150).IsFixedLength().IsRequired();
            this.Property(c => c.ReadTime).HasColumnType("nvarchar").HasMaxLength(300).IsFixedLength().IsRequired();
            this.Property(c => c.DateAdded).HasColumnType("datetime2").HasPrecision(0).IsRequired();

            this.HasRequired<User>(c => c.User).WithMany(c => c.Articles).HasForeignKey<Guid>(c => c.UserId).WillCascadeOnDelete(false);
           
        }
    }
}