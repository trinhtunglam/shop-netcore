using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideController : Controller
    {
        private readonly ISlideService _slideService;
        private readonly IMapper _mapper;
        private IHostingEnvironment _env;

        public SlideController(ISlideService slideService, IMapper mapper, IHostingEnvironment env)
        {
            _slideService = slideService;
            _env = env;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/slide")]
        public IActionResult Index()
        {
            var model = _slideService.GetAll();
            var result = _mapper.Map<IEnumerable<Slide>, IEnumerable<SlideViewModel>>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/slide/detail/{id}")]
        public IActionResult Detail(int id)
        {
            var model = _slideService.GetById(id);
            var result = _mapper.Map<Slide, SlideViewModel>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/slide/create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/slide/create")]
        public async Task<IActionResult> Create(SlideViewModel model)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        var uploads = Path.Combine(_env.WebRootPath, "images/slidebanner");

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

                var result = _mapper.Map<SlideViewModel, Slide>(model);
                _slideService.Insert(result);
                return RedirectToAction("Index", "Slide");
            }
            return View();
        }
    }
}