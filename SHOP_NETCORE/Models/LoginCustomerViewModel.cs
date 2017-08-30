using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class LoginCustomerViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Trường không được để trống")]
        [MaxLength(256)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Trường không được để trống")]
        [MinLength(6), MaxLength(16), DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
