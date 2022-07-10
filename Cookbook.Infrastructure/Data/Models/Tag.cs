using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Infrastructure.Data.Models
{
    public class Tag
    {
        public Tag()
        {
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public bool IsDeleted { get; set; }
    }
}