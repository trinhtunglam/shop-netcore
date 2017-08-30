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
    public class MenuViewComponent:ViewComponent
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuViewComponent(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _menuService.GetAll();
            var result = _mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(model);
            return View(result);
        }
    }
}
