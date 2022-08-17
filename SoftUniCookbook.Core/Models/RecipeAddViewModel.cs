using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class RecipeAddViewModel
    {
        [Required(ErrorMessage = "Please enter a {0}.")]
        [StringLength(50, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 6)]
        [Display(Name = "Recipe name")]
        public string Name { get; set; }

        [Display(Name = "Recipe Picture (optional)")]
        public IFormFile? Picture { get; set; }

        [Required(ErrorMessage = "Please enter some {0}.")]
        [StringLength(300, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 10)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter some {0}.")]
        [StringLength(1000, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 20)]
        public string Instructions { get; set; }

        [Required(ErrorMessage = "Please enter some {0}.")]
        [StringLength(500, ErrorMessage = "The {0} must be between {2} and {1} characters long.", MinimumLength = 3)]
        public string Ingredients { get; set; }

        [Display(Name="Serving size (2-20)")]
        [Required(ErrorMessage = "Please enter a {0}.")]
        [Range(2, 20, ErrorMessage = "The {0} must be between {2} and {1}.")]
        public byte ServingSize { get; set; }

        [Required]
        public string AuthorId { get; set; }
    }
}