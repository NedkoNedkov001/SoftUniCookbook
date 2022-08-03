using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.Core.Models
{
    public class DetailedUserViewModel
    {
        public string Id { get; set; }
        public byte[] Picture { get; set; }
        public IFormFile NewPicture { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public string PhoneNumber { get; set; }

    }
}
