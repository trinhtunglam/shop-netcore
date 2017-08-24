using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class ReceiptNoteViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int SupplierId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}
