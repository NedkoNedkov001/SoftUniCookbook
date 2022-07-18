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
            var users = await userService.GetUsers();

            return View(users);
        }

        public async Task<IActionResult> Roles(string id)
        {
            var user = await userService.GetUserById(id);
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
                });

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await userService.GetUserForEditById(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var errors = await userService.UpdateUser(model);

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
            if (await userService.DeleteUser(id))
            {
                ViewData[MessageConstant.SuccessMessage] = "User deleted successfully!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = new List<string> { "Could not delete user." };
            }
            return Redirect("/Admin/User/ManageUsers"); // TODO: Refresh page and show toastr
        }



        public async Task<IActionResult> CreateRole()
        {
            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Administrator",
            //});

            //await roleManager.CreateAsync(new IdentityRole()
            //{
            //    Name = "Moderator",
            //});

            return Ok();
        }
    }
}
