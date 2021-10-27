using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CARAPI.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string GrantType { get; set; }
        public string Scope { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
    }
}
