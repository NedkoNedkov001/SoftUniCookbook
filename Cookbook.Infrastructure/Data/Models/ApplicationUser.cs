using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Recipes = new HashSet<Recipe>();
            Favorites = new HashSet<UserFavorite>();
            SentMessages = new HashSet<Message>();
            ReceivedMessages = new HashSet<Message>();
            IsDeleted = false;
        }
        public byte[] Picture { get; set; }

        [Display(Name = "About user")]
        [StringLength(300, ErrorMessage = "{0} must be maximum {1} characters long.")]
        public string About { get; set; }

        public bool IsDeleted { get; set; }


        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<UserFavorite> Favorites { get; set; }
        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
    }
}
