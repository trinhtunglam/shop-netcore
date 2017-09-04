using AutoMapper;
using BUSINESS_OBJECTS;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using SHOP_NETCORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private readonly IProducerService _producerService;
        private readonly IMapper _mapper;

        public CategoryViewComponent(IProductCategoryService productCategoryService, IMapper mapper,
            IProducerService producerService,
            IProductService productService)
        {
            _productCategoryService = productCategoryService;
            _producerService = producerService;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _productService.GetAll();
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

            var productCategory = _productService.GroupBy();
            ViewBag.productCategory = _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(productCategory);

            var lstProducer1 = _productService.GroupByProducer();
            ViewBag.lstProducer1 = _mapper.Map<IEnumerable<Producer>, IEnumerable<ProducerViewModel>>(lstProducer1);

            var producer = _producerService.GetAll();
            var producerViewModel = _mapper.Map<IEnumerable<Producer>, IEnumerable<ProducerViewModel>>(producer);
            ViewBag.lstProducer = producerViewModel;

            return View(model);
        }
    }
}
