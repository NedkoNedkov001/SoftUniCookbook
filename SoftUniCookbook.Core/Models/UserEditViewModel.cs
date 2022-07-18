using Cookbook.Core.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class UserEditViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]

        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }

        //public byte[] Picture { get; set; }

    }
}
