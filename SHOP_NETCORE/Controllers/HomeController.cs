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
        private readonly ISlideService _slideService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public HomeController(IProductService productService, IMapper mapper,
            IProducerService producerService,
            ISupplierService supplierService,
            IProductCategoryService productCategoryService,
            ISlideService slideService)
        {
            _productService = productService;
            _producerService = producerService;
            _supplierService = supplierService;
            _slideService = slideService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var lstProductbyCategory = _productService.GetByCategoryHome();
            var lstProductbyCategoryViewModel = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProductbyCategory);
            ViewBag.lstProductbyCategory = lstProductbyCategoryViewModel;

            var lstProductbyPhone = _productService.GetByCategory(2);
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProductbyPhone);
            ViewBag.lstProductbyPhone = result;

            var category = _productService.GroupBy();
            var lstCategory = _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(category);
            ViewBag.lstCategory = lstCategory;

            var lstProductbyLaptop = _productService.GetByCategory(3);
            var result1 = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(lstProductbyLaptop);
            ViewBag.lstProductbyLaptop = result1;

            var slide = _slideService.GetAll();
            ViewBag.Slide = _mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(slide);

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [Route("contact.html")]
        public IActionResult Contact()
        {
            return View();
        }
    }
}
