using Cookbook.Core.Constants;
using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
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

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel model)
        {
            return RedirectToAction("Index", model.Keyword);
        }

        public async Task<IActionResult> AddFavorite(string userId, string recipeId)
        {
            await userService.AddFavoriteAsync(userId, recipeId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveFavorite(string userId, string recipeId)
        {
            await userService.RemoveFavoriteAsync(userId, recipeId);

            return RedirectToAction("Index");
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

        public async Task<IActionResult> Delete(string id) {
            if (await recipeService.DeleteRecipe(id))
            {
                TempData[MessageConstant.SuccessMessage] = "Deleted recipe successfully.";
            }
            else
            {
                TempData[MessageConstant.ErrorMessage] = "Could not delete recipe".ToArray();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Show(RecipeViewModel recipe)
        {
            //TODO: Add HTML and implement 
            return View(recipe);
        }
    }
}
