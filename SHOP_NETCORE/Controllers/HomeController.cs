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
            var category = _productService.GroupBy().Select(t => new ProductCategory { Id = t.Id, Name = t.Name });
            var lstCategory = _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(category);
            ViewBag.lstCategory = lstCategory;

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
