using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Net.Http.Headers;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController : Controller
    {
        private readonly IBannerService _bannerService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;

        public BannerController(IBannerService bannerService, IMapper mapper, IHostingEnvironment env,
            IProductCategoryService productCategoryService)
        {
            _bannerService = bannerService;
            _productCategoryService = productCategoryService;
            _env = env;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/banner")]
        public IActionResult Index()
        {
            var model = _bannerService.GetAll();
            var result = _mapper.Map<IEnumerable<Banner>, IEnumerable<BannerViewModel>>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/banner/create")]
        public IActionResult Create()
        {
            var productcategory = _productCategoryService.GetAll();
            ViewBag.CategoryId = new SelectList(productcategory, "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/banner/create")]
        public async Task<IActionResult> Create(BannerViewModel model)
        {
            var productcategory = _productCategoryService.GetAll();
            ViewBag.CategoryId = new SelectList(productcategory, "Id", "Name");

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        var uploads = Path.Combine(_env.WebRootPath, "images/banner");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.Trim('"');
                            System.Console.WriteLine(fileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                model.LinkImage = file.FileName;
                            }
                        }
                    }
                }

                var result = _mapper.Map<BannerViewModel, Banner>(model);
                _bannerService.Insert(result);
                return RedirectToAction("Index", "Banner");
            }
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/banner/detail/{id}")]
        public IActionResult Detail(int id)
        {
            var model = _bannerService.GetById(id);
            var result = _mapper.Map<Banner, BannerViewModel>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/banner/delete/{id}")]
        public IActionResult Delete(int id)
        {
            _bannerService.Delete(id);
            return View("Index");
        }
    }
}