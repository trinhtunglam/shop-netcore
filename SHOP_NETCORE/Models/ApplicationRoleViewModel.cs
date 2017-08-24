using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }
        [Display(Name ="Tên quyền")]
        public string RoleName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
        public int NumberOfUsers { get; set; }
    }
}
