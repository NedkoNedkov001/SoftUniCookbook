using Cookbook.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cookbook.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<ApplicationUser>()
            //    .HasMany(u => u.Messages)
            //    .WithOne(m => m.Sender)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .OnDelete(DeleteBehavior.NoAction);
                
            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Recipes)
                .WithOne(r => r.Author)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasMany(u => u.Comments)
                .WithOne(rc => rc.Recipe)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasOne(r => r.Author)
                .WithMany(u => u.Recipes)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserFavorite>()
                .HasKey(uf => new { uf.UserId, uf.RecipeId });

            builder.Entity<RecipeTag>()
                .HasKey(rt => new { rt.RecipeId, rt.TagId });

            builder.Entity<Rating>()
                .HasKey(r => new { r.UserId, r.RecipeId });
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<UserFavorite> UsersFavorites { get; set; }
        public DbSet<RecipeTag> RecipesTags { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }

}