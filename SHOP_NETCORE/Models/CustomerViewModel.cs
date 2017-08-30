using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage ="Trường không được để trống")]
        public string Name { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Trường không được để trống")]
        public string Address { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Trường không được để trống")]
        [MaxLength(256)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$|^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}", ErrorMessage = "Sai định dạng")]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Display(Name ="Mật khẩu")]
        [Required(ErrorMessage = "Trường không được để trống")]
        [MinLength(6), MaxLength(16), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MinLength(6), MaxLength(16), DataType(DataType.Password), Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu bắt buộc phải trùng nhau")]
        public string ConfirmPassword { get; set; }
        public bool Status { get; set; }
    }
}
