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
    public class CommentConfiguration:EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            this.HasKey<Guid>(c => c.CommentId).Property(c => c.CommentId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).HasColumnType("uniqueidentifier").IsRequired();


            this.Property(c => c.Body).HasColumnType("nvarchar").HasMaxLength(300).IsFixedLength().IsRequired();
            this.Property(c => c.DateAdded).HasPrecision(0).IsRequired();


            this.HasRequired<User>(c => c.User).WithMany(c => c.Comments).HasForeignKey<Guid>(c => c.UserId).WillCascadeOnDelete(false);
            this.HasRequired<Articles>(c => c.Article).WithMany(c => c.Comments).HasForeignKey<Guid>(c => c.ArticleId).WillCascadeOnDelete(false);
        }
    }
}