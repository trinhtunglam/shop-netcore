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
    public class InfomationViewComponent : ViewComponent
    {
        private readonly IInfomationService _infomationService;
        private readonly IMapper _mapper;

        public InfomationViewComponent(IInfomationService infomationService, IMapper mapper)
        {
            _infomationService = infomationService;
            _mapper = mapper;
        }

        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = _infomationService.GetAll();
            var result = _mapper.Map<IEnumerable<Infomation>, IEnumerable<InfomationViewModel>>(model);
            return View(result);
        }
    }
}
