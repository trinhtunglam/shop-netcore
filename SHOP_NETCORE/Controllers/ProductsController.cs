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
        private readonly IProducerService _producerService;
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper,
            IProductCategoryService productCategoryService,
            IProducerService producerService,
            ISupplierService supplierService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _producerService = producerService;
            _supplierService = supplierService;
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
            var producer = _producerService.GetSingleById(model.ProducerId);
            ViewBag.Producer = _mapper.Map<Producer, ProducerViewModel>(producer);

            var supplier = _supplierService.GetSingleById(model.SupplierId);
            ViewBag.Supplier = _mapper.Map<Supplier, SupplierViewModel>(supplier);

            var productRelated = _productService.GetProductRelated(id);
            ViewBag.ProductRelated = _mapper.Map<IEnumerable<Product>,IEnumerable < ProductViewModel >> (productRelated);

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

        [Route("sanpham-producer-{categoryId}-{producerId}")]
        public IActionResult GetProductByProducer(int categoryId, int producerId)
        {
            var model = _productService.GetByCategoryByProducer(categoryId,producerId);
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(result);
        }

        public IActionResult GetProductNew(int producerId)
        {
            var model = _productService.GetProductNew();
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(result);
        }

        public IActionResult GetProductBest(int producerId)
        {
            var model = _productService.GetProductBest();
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(result);
        }
    }
}