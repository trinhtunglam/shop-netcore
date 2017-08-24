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
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("products/detail/{id}")]
        public IActionResult ProductDetail(int id)
        {
            var model = _productService.GetSingleById(id);
            var result = _mapper.Map<Product, ProductViewModel>(model);
            return PartialView(result);
        }

        [Route("products/getcategory")]
        public IActionResult _Menu()
        {
            var model = _productCategoryService.GetAll();
            var result = _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return PartialView("_Menu",result);
        }
    }
}