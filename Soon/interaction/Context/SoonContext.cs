using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using Soon.interaction.Models;
using Soon.interaction.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using Soon.Migrations;

namespace Soon.interaction.Context
{
    public class SoonContext : DbContext
    {
        public SoonContext():base()
        {
            Database.SetInitializer<SoonContext>(new MigrateDatabaseToLatestVersion<SoonContext, Configuration>(useSuppliedContext: true));
        }

        public DbSet<Application> Application { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Articles> Article { get; set; }
        public DbSet<Comment> Comment { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();


            modelBuilder.Configurations.Add(new ApplicationConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ArticleConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());


            base.OnModelCreating(modelBuilder);
        }

    }
}