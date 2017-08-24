using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    [Serializable]
    public class ShoppingCartViewModel
    {
        public int ProductId { get; set; }
        public ProductViewModel product { get; set; }
        public int Quantity { get; set; }

    }
}
