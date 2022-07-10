using Cookbook.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SoftUniCookbook.Infrastructure.Data
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

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.SentMessages)
                .WithOne(um => um.Sender)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.ReceivedMessages)
                .WithOne(um => um.Receiver)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ApplicationUser>()
                .HasMany(u => u.Favorites)
                .WithOne(uf => uf.User)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasMany(u => u.Tags)
                .WithOne(rt => rt.Recipe)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Recipe>()
                .HasMany(u => u.Comments)
                .WithOne(rc => rc.Recipe)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<RecipeTag>(r =>
            {
                r.HasKey(rt => new { rt.RecipeId, rt.TagId });
            });
            //builder.Entity<UserMessage>(u =>
            //{
            //    u.HasKey(um => new { um.UserId, um.MessageId });
            //});
            builder.Entity<UserIngredient>(u =>
            {
                u.HasKey(ui => new { ui.UserId, ui.IngredientId });
            });
            builder.Entity<RecipeIngredient>(r =>
            {
                r.HasKey(ri => new { ri.RecipeId, ri.IngredientId });
            });
            builder.Entity<UserFavorite>(u =>
            {
                u.HasKey(uf => new { uf.UserId, uf.RecipeId });
            });
            builder.Entity<RecipeComment>(r =>
            {
                r.HasKey(rc => new { rc.RecipeId, rc.CommentId });
            });
        }

        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<UserIngredient> UsersIngredients { get; set; }
        //public DbSet<Message> Messages { get; set; }
        public DbSet<UserMessage> UsersMessages { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipesIngredients { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RecipeComment> RecipesComments { get; set; }
        public DbSet<UserFavorite> UsersFavorites { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipesTags { get; set; }
    }

}