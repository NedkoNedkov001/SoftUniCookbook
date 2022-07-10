using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Infrastructure.Data.Models
{
    public class Comment
    {
        public Comment()
        {
            Date = DateTime.Now;
            IsDeleted = false;
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Text { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}