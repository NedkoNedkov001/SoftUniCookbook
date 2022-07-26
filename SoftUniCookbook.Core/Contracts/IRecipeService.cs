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
        public Task<IEnumerable<RecipePreviewViewModel>> GetAllRecipes();
        public Task<IEnumerable<RecipePreviewViewModel>> GetFilteredRecipes(string keyword);
        public Task<Recipe> GetRecipeById(string recipeId);
    }
}
