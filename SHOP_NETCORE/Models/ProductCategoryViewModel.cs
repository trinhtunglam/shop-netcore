using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class ProductCategoryViewModel
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [Display(Name ="Tên danh mục")]
        public string Name { set; get; }
        [Required(ErrorMessage = "Tên định danh không được để trống")]
        [Display(Name = "Định danh")]
        public string Alias { set; get; }
        [Display(Name = "Mô tả")]
        public string Description { set; get; }
        [Display(Name = "Danh mục cha")]
        public int? ParentID { set; get; }
        public int? DisplayOrder { set; get; }
        public string Image { set; get; }
        public bool? HomeFlag { set; get; }
    }
}
