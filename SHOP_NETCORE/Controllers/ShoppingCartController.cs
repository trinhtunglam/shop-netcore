using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using COMMONS;
using Microsoft.AspNetCore.Http;
using SHOP_NETCORE.Models;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;

namespace SHOP_NETCORE.Controllers
{
    public class ShoppingCartController : Controller
    {
        string key = CommonConstants.SessionCart;
        string valueString = "";
        string user = CommonConstants.CustomerLogin;

        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IProductService productService, IMapper mapper,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [Route("giohang.html")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key) == null)
            {
                var value = new List<ShoppingCartViewModel>();
                HttpContext.Session.SetObject(key, value);
            }

            return View();
        }

        public JsonResult GetAll()
        {
            if (HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key) == null)
            {
                var value = new List<ShoppingCartViewModel>();
                HttpContext.Session.SetObject(key, value);
            }
            var cart = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);
            return Json(new
            {
                data = cart,
                status = true
            });
        }

        [HttpPost]
        public JsonResult Add(int productID)
        {
            var cart = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);
            // Kiểm tra xem chứa productID k

            var product = _productService.GetSingleById(productID);

            if (cart == null)
            {
                cart = new List<ShoppingCartViewModel>();
            }

            if (product.Quantity == 0)
            {
                return Json(new
                {
                    status = false,
                    message = "Sản phẩm này hiện đang hết hàng"
                });
            }

            if (cart.Any(t => t.ProductId == productID))
            {
                foreach (var item in cart)
                {
                    if (item.ProductId == productID)
                    {
                        item.Quantity += 1;
                    }
                }
            }
            else
            {
                ShoppingCartViewModel newItem = new ShoppingCartViewModel();
                newItem.ProductId = productID;
                //var product = _productService.GetById(productID);
                newItem.product = _mapper.Map<Product, ProductViewModel>(product);
                newItem.Quantity = 1;
                cart.Add(newItem);
            }
            HttpContext.Session.SetObject(key, cart);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult Update(string cartData)
        {
            var cartViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ShoppingCartViewModel>>(cartData);

            var cartSession = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);

            foreach (var item in cartSession)
            {
                foreach (var jitem in cartViewModel)
                {
                    if (item.ProductId == jitem.ProductId)
                    {
                        item.Quantity = jitem.Quantity;
                    }
                }
            }
            HttpContext.Session.SetObject(key,cartSession);

            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteAll()
        {
            var value = new List<ShoppingCartViewModel>();
            HttpContext.Session.SetObject(key, value);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public JsonResult DeleteItem(int productId)
        {
            var cartSession = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);
            if (cartSession != null)
            {
                cartSession.RemoveAll(t => t.ProductId == productId);
                HttpContext.Session.SetObject(key, cartSession);
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }

        [HttpPost]
        public JsonResult CreateOrrder(string orderViewModel)
        {
            var order = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderViewModel>(orderViewModel);
            var orderNew = new Order();
            orderNew = _mapper.Map<OrderViewModel, Order>(order);
            var cart = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);

            List<OrderDetail> orderDetail = new List<OrderDetail>();

            bool isEnough = false;

            foreach (var item in cart)
            {
                var detail = new OrderDetail();
                detail.ProductId = item.product.Id;
                detail.Quantitty = item.Quantity;
                detail.Price = item.product.PromotionPrice;
                orderDetail.Add(detail);

                isEnough = _productService.SubTractionProduct(item.product.Id, item.Quantity);
            }

            if (isEnough)
            {
                _orderService.Create(orderNew, orderDetail);

                var userSession = HttpContext.Session.GetObject<CustomerViewModel>(user);

                if (userSession == null)
                {
                    var customer = new Customer
                    {
                        Name = orderNew.CustomerName,
                        Address = orderNew.CustomerAddress,
                        Email = orderNew.CustomerEmail,
                        Phone = orderNew.CustomerMobile,
                        Status = false
                    };
                    _customerService.Add(customer);
                }

                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "không đủ hàng"
                });
            }
        }

        [HttpPost]
        public JsonResult CountItem()
        {
            var cartSession = HttpContext.Session.GetObject<List<ShoppingCartViewModel>>(key);
            int cart = cartSession.Count() +1;
            return Json(new
            {
                count = cart
            });
        }

        public JsonResult GetUser()
        {
            var userSession = HttpContext.Session.GetObject<CustomerViewModel>(user);
            if (userSession != null)
            {
                var userId = _customerService.GetSingleById(userSession.Id);
                return Json(new
                {
                    data = userId,
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
    }
}