using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("ReceiptNotes")]
    public class ReceiptNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Code { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { set; get; }

        //public virtual IEnumerable<ReceiptNoteDetail> ReceiptNoteDetails { set; get; }
    }
}
