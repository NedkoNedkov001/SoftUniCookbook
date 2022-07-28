using Cookbook.Core.Constants;
using Cookbook.Core.Contracts;
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
