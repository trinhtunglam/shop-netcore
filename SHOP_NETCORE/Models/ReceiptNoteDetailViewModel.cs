using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class ReceiptNoteDetailViewModel
    {
        public int Id { get; set; }
        public int ReceiptNodeId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public virtual ReceiptNoteViewModel ReceiptNote { set; get; }
        public virtual ProductViewModel Product { set; get; }
    }
}
