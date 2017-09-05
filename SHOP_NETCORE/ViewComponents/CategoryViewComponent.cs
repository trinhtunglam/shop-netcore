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
            var productCategory = _productService.GroupBy().Select(x=> new ProductCategory {Id=x.Id,Name=x.Name });
            ViewBag.productCategory = _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(productCategory);

            var lstProducer1 = _productService.GroupByProducer().Select(x => new Producer { Id = x.Id, Name = x.Name });
            ViewBag.lstProducer1 = _mapper.Map<IEnumerable<Producer>, IEnumerable<ProducerViewModel>>(lstProducer1);

            return View();
        }
    }
}
