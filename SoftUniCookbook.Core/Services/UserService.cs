using Cookbook.Core.Contracts;
using Cookbook.Core.Models;
using Cookbook.Infrastructure.Data.Models;
using Cookbook.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Cookbook.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationDbRepository repo;
        private readonly IRecipeService recipeService;

        public UserService(IApplicationDbRepository repo, IRecipeService recipeService)
        {
            this.repo = repo;
            this.recipeService = recipeService;
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsersAsync()
        {
            return await repo.All<ApplicationUser>()
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserListViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Picture = u.Picture
                })
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await repo.All<ApplicationUser>()
                .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());

            if (user == null)
            {
                throw new ArgumentNullException($"User {username} does not exist");
            }

            return user;
        }
        public async Task<UserViewModel> GetUserForViewByUsernameAsync(string username)
        {
            try
            {
                var user = await GetUserByUsernameAsync(username);
                UserViewModel userView = new UserViewModel()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Picture = user.Picture
                };
                return userView;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<HomeUserViewModel> GetHomeUserByUsernameAsync(string username)
        {
            try
            {
                var user = await GetUserByUsernameAsync(username);

                HomeUserViewModel homeUser = new HomeUserViewModel()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Favorites = (ICollection<UserFavorite>)GetUserFavoritesAsync(user.Id).Result
                };
                return homeUser;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            var user = await repo.All<ApplicationUser>()
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentNullException($"User with id '{id}' does not exist");
            }

            return user;
        }

        public async Task<UserEditViewModel> GetUserForEditByIdAsync(string id)
        {
            try
            {
                var user = await GetUserByIdAsync(id);

                var editUser = new UserEditViewModel()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    //Picture = user.Picture
                };
                return editUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var user = await repo.All<ApplicationUser>()
                .Where(u => u.NormalizedEmail == email.ToUpper())
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentNullException($"User with email '{email}' does not exist");
            }

            return user;
        }

        public async Task<UserViewModel> GetUserForViewByEmailAsync(string email)
        {
            try
            {
                var user = await GetUserByEmailAsync(email);

                var viewUser = new UserViewModel()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Picture = user.Picture
                };
                return viewUser;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<string>> UpdateUserAsync(UserEditViewModel model)
        {
            var errors = new HashSet<string>();

            if (await GetUserForViewByUsernameAsync(model.Username) != null &&
                (await GetUserForViewByUsernameAsync(model.Username)).Id != model.Id)
            {
                errors.Add("Username is already taken.");
            }
            if (await GetUserForViewByEmailAsync(model.Email) != null &&
                (await GetUserForViewByEmailAsync(model.Email)).Id != model.Id)
            {
                errors.Add("Email is already taken.");
            }
            if (errors.Count() == 0)
            {
                var user = await repo.GetByIdAsync<ApplicationUser>(model.Id);
                if (user != null || user.IsDeleted == false)
                {
                    user.UserName = model.Username;
                    user.NormalizedUserName = model.Username.ToUpper();
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.PhoneNumber = model.PhoneNumber;
                    user.PhoneNumberConfirmed = false;
                    //user.Picture = model.Picture;

                    await repo.SaveChangesAsync();
                }
            }

            return errors;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            user.IsDeleted = true;
            await repo.SaveChangesAsync();
            return (user != null);
        }

        public async Task<IEnumerable<UserFavorite>> GetUserFavoritesAsync(string id)
        {
            return await repo.All<UserFavorite>()
                .Where(uf => uf.UserId == id)
                .ToListAsync();
        }

        public async Task<bool> AddFavoriteAsync(string userId, string recipeId)
        {
            //throw new NotImplementedException();
            var user = await repo.GetByIdAsync<ApplicationUser>(userId);
            var recipe = await recipeService.GetRecipeById(recipeId);

            if (user != null && recipe != null)
            {
                UserFavorite userFavorite = new UserFavorite()
                {
                    UserId = user.Id,
                    RecipeId = recipe.Id
                };
                user.Favorites.Add(userFavorite);
                await repo.AddAsync(userFavorite);
                await repo.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
