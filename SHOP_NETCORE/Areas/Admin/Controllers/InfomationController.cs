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
    public class InfomationController : Controller
    {
        private readonly IInfomationService _infomationService;
        private readonly IMapper _mapper;

        public InfomationController(IInfomationService infomationService, IMapper mapper)
        {
            _infomationService = infomationService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/infomation")]
        public IActionResult Index()
        {
            var model = _infomationService.GetAll();
            var result = _mapper.Map<IEnumerable<Infomation>, IEnumerable<InfomationViewModel>>(model);
            return View(result);
        }
    }
}