using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class UserIngredient
    {
        [Required]
        [ForeignKey(nameof(User))]  
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [ForeignKey(nameof(Ingredient))]
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        [Required]
        [Range(0, 10000)]
        public double Quantity { get; set; }

    }
}
