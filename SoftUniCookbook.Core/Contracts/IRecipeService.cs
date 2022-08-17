using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Contracts
{
    public interface IRecipeService
    {
        public Task<IEnumerable<RecipePreviewViewModel>> GetAllRecipesAsync(int page);
        public Task<IEnumerable<RecipePreviewViewModel>> GetFilteredRecipesAsync(int page, string keyword);
        public Task<Recipe> GetRecipeByIdAsync(string recipeId);

        public Task AddRecipe(RecipeAddViewModel recipeToAdd);
        public Task<bool> DeleteRecipe(string recipeId);
        public Task<RecipeViewModel> GetRecipeForViewByIdAsync(string recipeId, UserViewModel user);
        public Task AddCommentAsync(string userId, string recipeId, string text);
        public Task DeleteCommentByIdAsync(string commentId);
    }
}
