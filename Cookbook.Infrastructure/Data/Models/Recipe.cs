﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cookbook.Infrastructure.Data.Models
{
    public class Recipe
    {
        public Recipe()
        {
            Id = Guid.NewGuid();
            Comments = new HashSet<Comment>();
            Tags = new HashSet<RecipeTag>();
            DateCreated = DateTime.Now;
            IsDeleted = false;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        [Required]
        public byte[] Picture { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Ingredients { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 20)]
        public string Instructions { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required]
        [Range(2, 20)]
        public byte ServingSize { get; set; }

        public ICollection<RecipeTag> Tags { get; set; }


        public ICollection<Comment> Comments { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}