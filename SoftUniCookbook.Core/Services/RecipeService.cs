using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using Cookbook.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IApplicationDbRepository repo;

        public RecipeService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task AddRecipe(RecipeAddViewModel recipeToAdd)
        {
            byte[] picture;
            if (recipeToAdd.Picture != null)
            {
                using (var stream = new MemoryStream())
                {
                    await recipeToAdd.Picture.CopyToAsync(stream);

                    picture = stream.ToArray();
                }
            }
            else
            {
                picture = ImageToByteArray("../SoftUniCookbook/wwwroot/img/recipe_default_img.jpg");
            }

            var recipe = new Recipe()
            {
                Name = recipeToAdd.Name,
                Description = recipeToAdd.Description,
                Instructions = recipeToAdd.Instructions,
                Ingredients = recipeToAdd.Ingredients,
                Picture = picture,
                ServingSize = recipeToAdd.ServingSize,
                AuthorId = recipeToAdd.AuthorId,
            };

            await repo.AddAsync(recipe);
            await repo.SaveChangesAsync();
        }

        private byte[] ImageToByteArray(string path)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(path,
                                           FileMode.Open,
                                           FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(path).Length;
            buff = br.ReadBytes((int)numBytes);
            return buff;
        }


        public async Task<IEnumerable<RecipePreviewViewModel>> GetAllRecipesAsync(int page)
        {
            return await repo.All<Recipe>()
                            .Where(r => r.IsDeleted == false)
                            .OrderByDescending(r => r.Score)
                            .Skip((page - 1) * 9)
                            .Take(9)
                            .Select(r => new RecipePreviewViewModel()
                            {
                                Id = r.Id,
                                Name = r.Name,
                                Picture = r.Picture,
                                Description = string.Join(" ", r.Description.Split().Take(12)),
                                ServeSize = r.ServingSize,
                                AuthorNickname = r.Author.UserName,
                                AuthorId = r.AuthorId
                            })
                            .ToListAsync();
        }

        public async Task<IEnumerable<RecipePreviewViewModel>> GetFilteredRecipesAsync(int page, string keyword)
        {
            return await repo.All<Recipe>()
                .Where(r => r.Name.Contains(keyword))
                .Where(r => r.IsDeleted == false)
                .OrderByDescending((r) => r.Score)
                .Skip((page - 1) * 9)
                .Take(9)
                .Select(r => new RecipePreviewViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Picture = r.Picture,
                    Description = string.Join(" ", r.Description.Split().Take(12)),
                    ServeSize = r.ServingSize,
                    AuthorNickname = r.Author.UserName,
                    AuthorId = r.AuthorId
                })
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeByIdAsync(string recipeId)
        {
            var recipe = await repo.All<Recipe>()
                .FirstOrDefaultAsync(r => r.Id == Guid.Parse(recipeId));

            if (recipe == null)
            {
                throw new ArgumentNullException($"Recipe with id '{recipeId}' does not exist");
            }

            return recipe;
        }

        public async Task<bool> DeleteRecipe(string recipeId)
        {
            var recipe = await GetRecipeByIdAsync(recipeId);

            recipe.IsDeleted = true;
            await repo.SaveChangesAsync();
            return (recipe != null);
        }

        public async Task<RecipeViewModel> GetRecipeForViewByIdAsync(string recipeId, UserOnRecipeShowViewModel user)
        {
            RecipeViewModel recipe = await repo.All<Recipe>()
                .Where(r => r.Id == Guid.Parse(recipeId))
                .Select(r => new RecipeViewModel()
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Author = r.Author,
                    DateCreated = r.DateCreated,
                    Picture = r.Picture,
                    ServingSize = r.ServingSize,
                    Score = r.Score,
                    Ingredients = r.Ingredients,
                    Instructions = r.Instructions,

                })
                .FirstOrDefaultAsync();

            recipe.Comments = await GetCommentsForViewByRecipeIdAsync(recipeId);
            recipe.Tags = await GetTagsForViewByRecipeIdAsync(recipeId);
            recipe.CurrentUser = user;
            return recipe;
        }

        private async Task<List<TagViewModel>> GetTagsForViewByRecipeIdAsync(string recipeId)
        {
            return await repo.All<RecipeTag>()
                .Where(rt => rt.RecipeId == Guid.Parse(recipeId))
                .Select(rt => new TagViewModel()
                {
                    Id = rt.TagId,
                    Name = rt.Tag.Name
                })
                .ToListAsync();
        }

        public async Task<List<CommentViewModel>> GetCommentsForViewByRecipeIdAsync(string recipeId)
        {
            return await repo.All<Comment>()
                .Where(c => c.IsDeleted == false)
                .Where(c => c.RecipeId == Guid.Parse(recipeId))
                .OrderByDescending(c => c.Date)
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    Date = c.Date,
                    Text = c.Text,
                    User = c.User,
                })
                .ToListAsync();
        }

        public async Task AddCommentAsync(string userId, string recipeId, string text)
        {
            Comment comment = new Comment()
            {
                UserId = userId,
                RecipeId = Guid.Parse(recipeId),
                Text = text
            };

            await repo.AddAsync(comment);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteCommentByIdAsync(string commentId)
        {
            Comment comment = await repo.All<Comment>()
                .Where(c => c.Id == Guid.Parse(commentId))
                .FirstOrDefaultAsync();

            comment.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public async Task UpvoteRecipeAsync(string userId, string recipeId)
        {
            Recipe recipe = await GetRecipeByIdAsync(recipeId);

            Rating rating = await repo.All<Rating>()
                .Where(r => r.UserId == userId)
                .Where(r => r.RecipeId == Guid.Parse(recipeId))
                .FirstOrDefaultAsync();
            if (rating == null)
            {
                rating = new Rating()
                {
                    RecipeId = Guid.Parse(recipeId),
                    UserId = userId,
                    Likes = true
                };
                recipe.Score++;
                await repo.AddAsync(rating);
            }
            else
            {
                if (rating.Likes == null)
                {
                    recipe.Score++;
                    rating.Likes = true;
                }
                else if (rating.Likes == false)
                {
                    recipe.Score += 2;
                    rating.Likes = true;
                }
                else if (rating.Likes == true)
                {
                    recipe.Score--;
                    rating.Likes = null;
                }
            }

            await repo.SaveChangesAsync();

        }     
        
        public async Task DownvoteRecipeAsync(string userId, string recipeId)
        {
            Recipe recipe = await GetRecipeByIdAsync(recipeId);

            Rating rating = await repo.All<Rating>()
                .Where(r => r.UserId == userId)
                .Where(r => r.RecipeId == Guid.Parse(recipeId))
                .FirstOrDefaultAsync();

            if (rating == null)
            {
                rating = new Rating()
                {
                    RecipeId = Guid.Parse(recipeId),
                    UserId = userId,
                    Likes = false
                };
                recipe.Score--;
                await repo.AddAsync(rating);
            }
            else
            {
                if (rating.Likes == null)
                {
                    recipe.Score--;
                    rating.Likes = false;
                }
                else if (rating.Likes == true)
                {
                    recipe.Score -= 2;
                    rating.Likes = false;
                }
                else if (rating.Likes == false)
                {
                    recipe.Score++;
                    rating.Likes = null;
                }
            }
            await repo.SaveChangesAsync();
        }
    }
}
