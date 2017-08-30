using COMMONS;
using Microsoft.AspNetCore.Mvc;
using SHOP_NETCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        string key = CommonConstants.SessionCart;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var cart = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);
            return View(cart);
        }
    }
}
