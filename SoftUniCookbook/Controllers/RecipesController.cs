using Cookbook.Core.Constants;
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
                home.Recipes = await recipeService.GetFilteredRecipesAsync(keyword);
            }
            else
            {
                home.Recipes = await recipeService.GetAllRecipesAsync();
            }


            if (User.Identity.IsAuthenticated)
            {
                home.User = await userService.GetHomeUserByUsernameAsync(User.Identity.Name);
            }
            else
            {
                home.User = null;
            }

            return View(home);
        }

        public async Task<IActionResult> Add(string userId)
        {
            var model = new RecipeAddViewModel()
            {
                AuthorId = userId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RecipeAddViewModel recipeToAdd)
        {
            if (ModelState.IsValid)
            {
                if (Request.Form.Files.Any())
                {
                    recipeToAdd.Picture = Request.Form.Files[0];
                }

                try
                {
                    await recipeService.AddRecipe(recipeToAdd);
                    TempData[MessageConstant.SuccessMessage] = "Added recipe successfully.";
                    return RedirectToAction("Index", "Recipes");
                }
                catch (Exception)
                {
                    TempData[MessageConstant.ErrorMessage] = "Could not add recipe".ToArray();
                    return View(recipeToAdd);
                }
            }
            else
            {
                return View(recipeToAdd);
            }
        }

        public async Task<IActionResult> View(RecipeViewModel recipe)
        {
            return View(recipe);
        }
    }
}
