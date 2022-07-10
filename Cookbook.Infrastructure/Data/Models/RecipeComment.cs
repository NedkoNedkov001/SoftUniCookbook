using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class RecipeComment
    {
        [Required]
        [ForeignKey(nameof(Recipe))]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [Required]
        [ForeignKey(nameof(Comment))]
        public Guid CommentId { get; set; }
        public Comment Comment { get; set; }

    }
}
