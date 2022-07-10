using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class UserMessage
    {
        public UserMessage()
        {
            IsRead = false;
            Date = DateTime.Now;
            IsDeleted = false;
        }
        public Guid Id { get; set; }


        [ForeignKey(nameof(Sender))]
        public string? SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        public string? ReceiverId { get; set; }
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
