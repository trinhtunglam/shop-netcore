using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper, IOrderService orderService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _mapper = mapper;
        }

        [Route("admin/customer")]
        public IActionResult Index()
        {
            var model = _customerService.GetCustomerLogin();
            var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(model);
            return View(result);
        }

        [Route("admin/customer/guest")]
        public IActionResult Guest()
        {
            var model = _customerService.GetCustomerGuest();
            var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(model);
            return View(result);
        }

        [Route("admin/customer/history/{email}")]
        public IActionResult GetCustomerByEmail(string email)
        {
            var model = _orderService.GetOrderByCustomerEmail(email);
            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
            return View(result);
        }
    }
}