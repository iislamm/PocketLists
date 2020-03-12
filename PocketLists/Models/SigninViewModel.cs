using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketLists.Models
{
    public class SigninViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ErrorMessage { get; set; }
    }
}
