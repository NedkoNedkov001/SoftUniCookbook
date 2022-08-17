using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class RecipePreviewViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Picture { get; set; }

        public string Description { get; set; }

        public byte ServeSize { get; set; }

        public string AuthorId { get; set; }

        public string AuthorNickname { get; set; }
    }
}
