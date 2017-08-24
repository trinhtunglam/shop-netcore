using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;

namespace SHOP_NETCORE.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProducerService _producerService;
        private readonly ISupplierService _supplierService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public HomeController(IProductService productService, IMapper mapper,
            IProducerService producerService,
            ISupplierService supplierService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _producerService = producerService;
            _supplierService = supplierService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lstProductbyCategory = _productService.GetByCategory(3).Take(8);
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProductbyCategory);
            ViewBag.LstProductByCategory = result;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
