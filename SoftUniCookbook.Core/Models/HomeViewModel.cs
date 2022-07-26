using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class HomeViewModel
    {
        public HomeUserViewModel User { get; set; }

        public IEnumerable<RecipePreviewViewModel> Recipes { get; set; }
    }
}
