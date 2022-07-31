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
        public Task<IEnumerable<RecipePreviewViewModel>> GetAllRecipesAsync();
        public Task<IEnumerable<RecipePreviewViewModel>> GetFilteredRecipesAsync(string keyword);
        public Task<Recipe> GetRecipeByIdAsync(string recipeId);

        public Task AddRecipe(RecipeAddViewModel recipeToAdd);
    }
}
