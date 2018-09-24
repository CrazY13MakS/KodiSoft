using FeedApi.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedApi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Feed>()
            .Property(b => b.CreatedAt)
            .HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Entity<FeedCollection>()
          .Property(b => b.CreatedAt)
          .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Entity<FeedCollectionsFeed>()
        .Property(b => b.CreatedAt)
        .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Entity<FeedCollection>()
             .Property(b => b.LastUpdate)
                .ValueGeneratedOnUpdate();
        }

        public virtual DbSet<Feed> Feeds { get; set; }
        public virtual DbSet<FeedCollection> FeedCollections { get; set; }
        public virtual DbSet<FeedCollectionsFeed> FeedCollectionsFeeds { get; set; }

    }
}
