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
    public class ProductNewViewComponent:ViewComponent
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductNewViewComponent(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _productService.GetProductNew().Take(5).Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                Images = x.Images,
                Price = x.Price,
                PromotionPrice = x.PromotionPrice
            });
            var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(result);
        }
    }
}
