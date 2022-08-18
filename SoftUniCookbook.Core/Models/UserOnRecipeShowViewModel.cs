using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class UserOnRecipeShowViewModel
    {
        public string Id { get; set; }

        public ICollection<Guid> Favorites { get; set; }

        public bool? LikedRecipe { get; set; }
    }
}
