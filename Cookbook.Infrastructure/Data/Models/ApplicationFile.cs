using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Infrastructure.Data.Models
{
    public class ApplicationFile
    {
        public ApplicationFile()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }
    }
}
