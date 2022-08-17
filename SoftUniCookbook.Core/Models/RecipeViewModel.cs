using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class RecipeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set; }

        public string Instructions { get; set; }

        public ApplicationUser Author { get; set; }

        public byte ServingSize { get; set; }

        public ICollection<RecipeTag> Tags { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsDeleted { get; set; }
    }
}
