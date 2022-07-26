using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;

        public RecipesController(IUserService userService, IRecipeService recipeService)
        {
            this.userService = userService;
            this.recipeService = recipeService;
        }

        public async Task<IActionResult> Index(string keyword)
        {
            var home = new HomeViewModel();

            if (keyword != null)
            {
                home.Recipes = await recipeService.GetFilteredRecipes(keyword);
            }
            else
            {
                home.Recipes = await recipeService.GetAllRecipes();
            }


            if (User.Identity.IsAuthenticated)
            {
                home.User = await userService.GetHomeUserByUsername(User.Identity.Name);
            }
            else
            {
                home.User = null;
            }

            return View(home);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

    }
}
