using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("ProductTags")]
    public class ProductTag
    {
        //[Key]
        //[Column(Order = 1)]
        public int ProductId { set; get; }


        //[Column(TypeName = "varchar")]
        //[MaxLength(50)]
        public string TagId { set; get; }

        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { set; get; }
    }
}
