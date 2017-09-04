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
    public class ProductByCategoryViewComponent:ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductByCategoryViewComponent(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            var model = _productService.GetByCategory(categoryId);
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(result);
        }
    }
}
