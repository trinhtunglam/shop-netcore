using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        [Display(Name ="Ghi nhớ")]
        public bool RememberMe { get; set; }

    }
}
