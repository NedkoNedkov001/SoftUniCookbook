using Cookbook.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
