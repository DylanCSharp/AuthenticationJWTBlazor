using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Client.Shared.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Field is required"), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public string Password { get; set; }
    }
}
