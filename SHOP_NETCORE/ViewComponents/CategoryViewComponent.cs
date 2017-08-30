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
        private readonly IProducerService _producerService;
        private readonly IMapper _mapper;

        public CategoryViewComponent(IProductCategoryService productCategoryService, IMapper mapper,
            IProducerService producerService)
        {
            _productCategoryService = productCategoryService;
            _producerService = producerService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model =  _productCategoryService.GetAll();
            var result =  _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

            var producer = _producerService.GetAll();
            var producerViewModel = _mapper.Map<IEnumerable<Producer>, IEnumerable<ProducerViewModel>>(producer);
            ViewBag.lstProducer = producerViewModel;

            return View(result);
        }
    }
}
