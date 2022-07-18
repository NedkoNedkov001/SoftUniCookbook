using Cookbook.Core.Contracts;
using Cookbook.Infrastructure.Data.Models;
using Cookbook.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.CustomAttributes
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        private readonly IUserService userService;
        public UniqueUsernameAttribute(IUserService userService, string errorMessage = "Username is already taken.")
        {
            this.userService = userService;
            ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (userService.GetUserForViewByUsername((string)value) == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }

}
