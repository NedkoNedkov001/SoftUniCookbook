using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 1)]
        public string Unit { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
