using Cookbook.Controllers;
using Cookbook.Core.Constants;
using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Microsoft.AspNetCore.Mvc;
using SoftUniCookbook.Models;
using System.Diagnostics;

namespace SoftUniCookbook.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly IRecipeService recipeService;

        public HomeController(ILogger<HomeController> logger, IUserService userService, IRecipeService recipeService)
        {
            this.userService = userService;
            this.recipeService = recipeService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string keyword = null)
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
                home.User = await userService.GetHomeUserByUsernameAsync(User.Identity.Name);
            }
            else
            {
                home.User = null;
            }

            return View(home);

        }

        public async Task<IActionResult> AddToFavorite(string userId, string recipeId)
        {
            await userService.AddFavoriteAsync(userId, recipeId);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}