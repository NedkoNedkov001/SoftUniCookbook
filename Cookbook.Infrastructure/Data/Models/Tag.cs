using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Infrastructure.Data.Models
{
    public class Tag
    {
        public Tag()
        {
            IsDeleted = false;
            Recipes = new HashSet<RecipeTag>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<RecipeTag> Recipes { get; set; }
    }
}