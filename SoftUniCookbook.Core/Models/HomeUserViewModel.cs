using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class HomeUserViewModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public ICollection<UserFavorite> Favorites { get; set; }

    }
}
