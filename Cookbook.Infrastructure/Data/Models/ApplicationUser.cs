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
            Favorites = new HashSet<UserFavorite>();
            Cart = new HashSet<UserIngredient>();
            SentMessages = new HashSet<UserMessage>();
            ReceivedMessages = new HashSet<UserMessage>();
            IsDeleted = false;
        }
        public byte[] Picture { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<UserFavorite> Favorites { get; set; }

        public ICollection<UserIngredient> Cart { get; set; }

        public ICollection<UserMessage> SentMessages { get; set; }

        public ICollection<UserMessage> ReceivedMessages { get; set; }
    }
}
