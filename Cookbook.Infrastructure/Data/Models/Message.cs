using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Infrastructure.Data.Models
{
    public class Message
    {
        public Message()
        {
            IsRead = false;
            Date = DateTime.Now;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [Required]
        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 5)]
        public string Text { get; set; }

        [Required]
        public bool IsRead { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}