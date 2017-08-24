using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class ProducerViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên nhà sản xuất không được để trống")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [Display(Name = "Tên nhà sản xuất")]
        public string Name { get; set; }
        [Display(Name = "Thông tin")]
        public string Infomation { get; set; }
        [Display(Name = "Logo")]
        public string Logo { get; set; }
        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }
    }
}
