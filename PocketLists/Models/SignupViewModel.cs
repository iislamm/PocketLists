using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketLists.Models
{
    public class SignupViewModel
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string ErrorMessage { get; set; }
    }
}
