using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Client.Shared.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Field is required"), EmailAddress(ErrorMessage = "Must be a valid email address")]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        [Required(ErrorMessage = "Field is required")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
