using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class RegisterViewModel
    {
        [Required, MaxLength(16)]
        public string FullName { get; set; }

        [Required, MaxLength(256)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Please enter a valid email address or phone number")]
        public string Email { get; set; }

        [Required, MaxLength(16)]
        public string UserName { get; set; }

        [Required, MinLength(6), MaxLength(16), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, MinLength(6), MaxLength(16), DataType(DataType.Password), Display(Name = "Password Confirm")]
        [Compare("Password", ErrorMessage = "The password does not match Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
