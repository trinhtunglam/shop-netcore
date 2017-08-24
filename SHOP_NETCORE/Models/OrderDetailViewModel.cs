using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class OrderDetailViewModel
    {
        public int OrderId { set; get; }
        public int ProductId { set; get; }
        public int Quantitty { set; get; }
        public decimal Price { get; set; }
        public virtual ProductViewModel Product { set; get; }
    }
}
