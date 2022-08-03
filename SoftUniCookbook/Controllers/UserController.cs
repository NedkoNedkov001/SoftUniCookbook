using Cookbook.Core.Constants;
using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
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

        public async Task<IActionResult> Edit()
        {
            UserEditViewModel editUser = null;
            try
            {
                editUser = await userService.GetUserForEditByUsernameAsync(User.Identity.Name);
            }
            catch (Exception)
            {

            }

            return View(editUser);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel editUser)
        {
            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            if (Request.Form.Files.Any())
            {
                editUser.NewPicture = Request.Form.Files[0];
            }

            if (editUser.About == null)
            {
                editUser.About = "";
            }

            var errors = await userService.UpdateUserAsync(editUser);

            if (errors.Count() == 0)
            {
                ViewData[MessageConstant.SuccessMessage] = "User edited successfully!";
            }
            else
            {
                ViewData[MessageConstant.ErrorMessage] = errors;
            }

            return RedirectToAction("Edit");

            //if (ModelState.IsValid)
            //{
            //    if (Request.Form.Files.Any())
            //    {
            //        editUser.NewPicture = Request.Form.Files[0];
            //    }

            //    try
            //    {
            //        await userService.UpdateUserAsync(editUser);
            //        TempData[MessageConstant.SuccessMessage] = "User edited successfully.";
            //        return RedirectToAction($"Profile?{editUser.Username}");
            //    }
            //    catch (Exception)
            //    {
            //        TempData[MessageConstant.ErrorMessage] = "Could not add recipe".ToArray();
            //        return View(editUser);
            //    }
            //}
            //else
            //{
            //    return View(editUser);
            //}
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    await roleManager.CreateAsync(new IdentityRole()
        //    {
        //        Name = UserConstants.Roles.Moderator,
        //    });

        //    await roleManager.CreateAsync(new IdentityRole()
        //    {
        //        Name = UserConstants.Roles.Administrator
        //    });

        //    await roleManager.CreateAsync(new IdentityRole()
        //    {
        //        Name = UserConstants.Roles.User
        //    });

        //    var user = await userService.GetUserByUsername(User.Identity.Name);
        //    await userManager.AddToRoleAsync(user, UserConstants.Roles.Administrator);

        //    return Ok();
        //}

    }
}
