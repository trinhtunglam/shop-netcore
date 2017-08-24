using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("ReceiptNoteDetails")]
    public class ReceiptNoteDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ReceiptNodeId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("ReceiptNodeId")]
        public virtual ReceiptNote ReceiptNote { set; get; }
        [ForeignKey("ProductId")]
        public virtual Product Product { set; get; }

    }
}
