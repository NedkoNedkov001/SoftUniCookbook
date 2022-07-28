using Cookbook.Core.Constants;
using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Cookbook.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserService userService;

        public UserController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ManageUsers()
        {
            var users = await userService.GetUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserByIdAsync(id);
            var model = new UserRolesViewModel()
            {
                UserId = user.Id,
                Username = user.UserName
            };


            ViewBag.RoleItems = roleManager.Roles
                .ToList()
                .Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Roles(UserRolesViewModel model)
        {
            var user = await userService.GetUserByIdAsync(model.UserId);
            var userRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.RoleNames?.Length > 0)
            {
                await userManager.AddToRolesAsync(user, model.RoleNames);
            }

            return RedirectToAction(nameof(ManageUsers));
        }



        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.GetUserForEditByIdAsync(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var errors = await userService.UpdateUserAsync(model);

            if (errors.Count() == 0)
            {
                ViewData[MessageConstant.SuccessMessage] = "User edited successfully!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = errors;
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await userService.DeleteUserAsync(id);

            return RedirectToAction(nameof(ManageUsers));
        }



    }
}
