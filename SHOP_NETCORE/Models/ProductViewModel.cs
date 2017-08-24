using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    [Serializable]
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Mã sản phẩm không được để trống")]
        [MinLength(3, ErrorMessage = "Tối thiểu 3 ký tự")]
        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }
        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }
        [Display(Name = "Định danh")]
        public string Alias { set; get; }
        [Display(Name = "Giá bán")]
        public decimal Price { get; set; }
        [Display(Name = "Giá khuyến mãi")]
        public decimal PromotionPrice { set; get; }
        [Display(Name = "Giá nhập")]
        public decimal OriginalPrice { set; get; }
        [Display(Name = "Bảo hành")]
        public int? Warranty { set; get; }
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        [Display(Name = "Hình ảnh")]
        public string Images { get; set; }
        [Display(Name = "Nội dung")]
        public string Content { get; set; }
        public string MoreImages { set; get; }
        [Display(Name = "Sản phẩm HOT")]
        public bool? HotFlag { set; get; }
        [Display(Name = "Trạng thái")]
        public bool? Status { set; get; }
        public IFormFile File { get; set; }

        [Display(Name = "Danh mục")]
        public int CategoryId { set; get; }
        [Display(Name = "Nhà sản xuất")]
        public int ProducerId { get; set; }
        [Display(Name = "Nhà cung cấp")]
        public int SupplierId { get; set; }
    }
}
