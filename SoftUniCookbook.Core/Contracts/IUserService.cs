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
        Task<IEnumerable<UserListViewModel>> GetUsers();
        Task<ApplicationUser> GetUserById(string id);
        Task<UserEditViewModel> GetUserForEditById(string id);
        Task<UserViewModel> GetUserForViewByUsername(string username);
        Task<UserViewModel> GetUserForViewByEmail(string email);
        Task<IEnumerable<string>> UpdateUser(UserEditViewModel model);
        Task<bool> DeleteUser(string id);
    }
}
