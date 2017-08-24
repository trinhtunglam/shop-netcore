using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("CouponDetails")]
    public class CouponDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //public int ProductId { set; get; }
        public int CouponId { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { set; get; }
        [ForeignKey("CouponId")]
        public virtual Coupon Coupon { set; get; }
    }
}
