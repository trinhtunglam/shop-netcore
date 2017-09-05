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
    public class BannerHomeViewComponent:ViewComponent
    {
        private readonly IBannerService _bannerService;
        private readonly IMapper _mapper;

        public BannerHomeViewComponent(IBannerService bannerService, IMapper mapper)
        {
            _bannerService = bannerService;
            _mapper = mapper;
        }


        public async Task<IViewComponentResult> InvokeAsync(int categoryId)
        {
            var model = _bannerService.GetBannerByCategory(categoryId).Select(x=> new Banner {LinkImage=x.LinkImage });
            var result = _mapper.Map<IEnumerable<Banner>, IEnumerable<BannerViewModel>>(model);
            return View(result);
        }
    }
}
