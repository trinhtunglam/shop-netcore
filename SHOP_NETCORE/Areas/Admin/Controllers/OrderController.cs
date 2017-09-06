using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;
using Microsoft.AspNetCore.Authorization;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [Route("admin/order")]
        public IActionResult Index()
        {
            var model = _orderService.GetAll();
            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
            return View(result);
        }

        [Route("admin/orderdetail/{id}")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult OrderDetail(int id)
        {
            var model = _orderService.GetByOrderId(id);
            var result = _mapper.Map<IEnumerable<OrderDetail>, IEnumerable<OrderDetailViewModel>>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult SelectOrder(int id)
        {
            var model = _orderService.GetBySelectId(id);
            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
            return PartialView("_OrderPartial",result);
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult ActiveOrder(int id)
        {
            var modelId = _orderService.GetById(id);
            modelId.Status = true;
            var orderResult = _orderService.UpdateResult(modelId);

            var model = _orderService.GetAll();

            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
            return PartialView("_OrderPartial", result);
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult SearchOrder(string searchString)
        {
            var model = _orderService.GetAll(searchString);
            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
            return PartialView("_OrderPartial", result);
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult DeleteOrder(int id)
        {
           _orderService.Delete(id);
            var model = _orderService.GetAll();

            var result = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(model);
            return PartialView("_OrderPartial", result);
        }
    }
}