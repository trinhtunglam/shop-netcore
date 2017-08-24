using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class SupplierViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Tên nhà cung cấp không được để trống")]
        [MinLength(3,ErrorMessage ="Tối thiểu 3 ký tự")]
        [Display(Name ="Tên nhà cung cấp")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
    }
}
