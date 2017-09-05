using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class BannerViewModel
    {
        public int Id { get; set; }
        [Display(Name="Tên ảnh")]
        public string Name { get; set; }
        public string LinkImage { get; set; }
        public int CategoryId { get; set; }
        [Display(Name = "Hiển thị")]
        public bool Status { get; set; }
        public virtual ProductCategoryViewModel ProductCategory { set; get; }
    }
}
