using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class RecipeIngredient
    {
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [Required]
        [ForeignKey(nameof(Ingredient))]
        public Guid IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }

        [Required]
        [Range(0, 10000)]
        public double Quantity { get; set; }
    }
}
