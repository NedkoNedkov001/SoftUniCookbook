using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cookbook.Core.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserListViewModel>> GetUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<HomeUserViewModel> GetHomeUserByUsernameAsync(string username);
        Task<UserEditViewModel> GetUserForEditByIdAsync(string id);
        Task<UserViewModel> GetUserForViewByUsernameAsync(string username);
        Task<UserViewModel> GetUserForViewByEmailAsync(string email);
        Task<IEnumerable<string>> UpdateUserAsync(UserEditViewModel model);
        Task<bool> DeleteUserAsync(string id);
        Task<ICollection<Guid>> GetUserFavoritesAsync(string id);
        Task<DetailedUserViewModel> GetDetailedUserByUsernameAsync(string username);
        Task<UserEditViewModel> GetUserForEditByUsernameAsync(string username);
        Task<bool> AddFavoriteAsync(string userId, string recipeId);
        Task<bool> RemoveFavoriteAsync(string userId, string recipeId);
        Task<UserOnRecipeShowViewModel> GetUserForRecipeShowByUsernameAsync(string username, string recipeId);
    }
}
