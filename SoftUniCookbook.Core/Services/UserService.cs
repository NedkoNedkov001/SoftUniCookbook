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
       
        public UserService(IApplicationDbRepository repo)
        {
            this.repo = repo;
        }

        public async Task<UserEditViewModel> GetUserForEditById(string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            return new UserEditViewModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                //Picture = user.Picture
            };
        }

        public async Task<UserViewModel> GetUserForViewByUsername(string username)
        {
            return await repo.All<ApplicationUser>()
                .Where(u => u.UserName == username)
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Picture = u.Picture
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserViewModel> GetUserForViewByEmail(string email)
        {
            return await repo.All<ApplicationUser>()
                .Where(u => u.Email == email)
                .Where(u => u.IsDeleted == false)
                .Select(u => new UserViewModel()
                {
                    Id = u.Id,
                    Username = u.UserName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    Picture = u.Picture
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserListViewModel>> GetUsers()
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

        public async Task<IEnumerable<string>> UpdateUser(UserEditViewModel model)
        {
            var errors = new HashSet<string>();

            if (await GetUserForViewByUsername(model.Username) != null && (await GetUserForViewByUsername(model.Username)).Id != model.Id)
            {
                errors.Add("Username is already taken.");
            }
            if (await GetUserForViewByEmail(model.Email) != null && (await GetUserForViewByEmail(model.Email)).Id != model.Id)
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

        public async Task<bool> DeleteUser(string id)
        {
            var user = await repo.GetByIdAsync<ApplicationUser>(id);

            user.IsDeleted = true;
            await repo.SaveChangesAsync();
            return (user != null);
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await repo.GetByIdAsync<ApplicationUser>(id);
        }
    }
}
