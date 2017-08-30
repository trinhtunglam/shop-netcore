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

            //var result = new ProductViewModel();
            //result.Id = model.Id;
            //result.Name = model.Name;
            //result.ProductCode = model.ProductCode;
            //result.Images = model.Images;
            //result.Price = model.Price;
            //result.PromotionPrice = model.PromotionPrice.Value;
            //result.SupplierViewModel.Name =model.Supplier.Name;
            //result.ProducerViewModel.Name =model.Producer.Name;


            var result = _mapper.Map<Product, ProductViewModel>(model);
            return View(result);
        }

        [Route("sanpham-category-{categotyId}")]
        public IActionResult GetProductByCategory(int categotyId)
        {
            var model = _productService.GetByCategory(categotyId);
            var result = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductViewModel>>(model);
            return View(result);
        }

        [Route("sanpham-producer-{producerId}")]
        public IActionResult GetProductByProducer(int producerId)
        {
            var model = _productService.GetByProducer(producerId);
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(result);
        }
    }
}