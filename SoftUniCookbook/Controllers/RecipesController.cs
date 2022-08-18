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
        public async Task<IActionResult> Index(int page, string keyword)
        {
            var home = new HomeViewModel();

            if (keyword != null)
            {
                home.Recipes = await recipeService.GetFilteredRecipesAsync(page, keyword);
            }
            else
            {
                home.Recipes = await recipeService.GetAllRecipesAsync(page);
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

        //[HttpPost]
        //public async Task<IActionResult> Index(HomeViewModel model)
        //{
        //    var r = 0;
        //    //return RedirectToAction("Index", model.Keyword);
        //    return RedirectToAction("Index", "Recipes", new RouteValueDictionary { { "page", 1 }, { "keyword", model.Keyword } });
        //}

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            return RedirectToAction("Index", "Recipes", new RouteValueDictionary { { "page", 1 }, { "keyword", keyword } });
        }

        public async Task<IActionResult> AddFavorite(string userId, string recipeId)
        {
            await userService.AddFavoriteAsync(userId, recipeId);

            return RedirectToAction("Index", "Recipes", new RouteValueDictionary { { "page", 1 } });
        }

        public async Task<IActionResult> RemoveFavorite(string userId, string recipeId)
        {
            await userService.RemoveFavoriteAsync(userId, recipeId);

            return RedirectToAction("Index", "Recipes", new RouteValueDictionary { { "page", 1 } });
        }

        public async Task<IActionResult> AddFavoriteOpen(string userId, string recipeId)
        {
            await userService.AddFavoriteAsync(userId, recipeId);

            return RedirectToAction("Show", "Recipes", new RouteValueDictionary { { "recipeId", recipeId } });
        }

        public async Task<IActionResult> RemoveFavoriteOpen(string userId, string recipeId)
        {
            await userService.RemoveFavoriteAsync(userId, recipeId);

            return RedirectToAction("Show", "Recipes", new RouteValueDictionary { { "recipeId", recipeId } });
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
            return RedirectToAction("Index", "Recipes", new RouteValueDictionary { { "page", 1 } });
        }

        public async Task<IActionResult> Show(string recipeId)
        {
            UserOnRecipeShowViewModel user = await userService.GetUserForRecipeShowByUsernameAsync(User.Identity.Name, recipeId);
            RecipeViewModel recipe = await recipeService.GetRecipeForViewByIdAsync(recipeId, user);

            return View(recipe);
        }

        public async Task<IActionResult> DeleteComment(string recipeId, string commentId)
        {
            await recipeService.DeleteCommentByIdAsync(commentId);

            return RedirectToAction("show", "recipes", new RouteValueDictionary { { "recipeId", recipeId } });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment()
        {
            string userId = Request.Form["NewComment.UserId"];
            string recipeId = Request.Form["NewComment.RecipeId"];
            string text = Request.Form["NewComment.Text"];

            await recipeService.AddCommentAsync(userId, recipeId, text);

            return RedirectToAction("show", "recipes", new RouteValueDictionary { { "recipeId", recipeId } });
        }

        public async Task<IActionResult> Upvote(string userId, string recipeId)
        {
            await recipeService.UpvoteRecipeAsync(userId, recipeId);

            return RedirectToAction("show", "recipes", new RouteValueDictionary { { "recipeId", recipeId } });
        }
        public async Task<IActionResult> Downvote(string userId, string recipeId)
        {
            await recipeService.DownvoteRecipeAsync(userId, recipeId);

            return RedirectToAction("show", "recipes", new RouteValueDictionary { { "recipeId", recipeId } });
        }
    }
}
