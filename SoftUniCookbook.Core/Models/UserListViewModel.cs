using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public byte[] Picture { get; set; }
    }
}
