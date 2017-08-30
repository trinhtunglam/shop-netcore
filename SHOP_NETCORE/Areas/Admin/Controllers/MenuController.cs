using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuController(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/menu")]
        public IActionResult Index()
        {
            var model = _menuService.GetAll();
            var result = _mapper.Map<IEnumerable<Menu>, IEnumerable<MenuViewModel>>(model);
            return View(result);
        }
    }
}