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

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProducerController : Controller
    {
        private readonly IProducerService _producerService;
        private readonly IMapper _mapper;

       
        public ProducerController(IProducerService producerService, IMapper mapper)
        {
            _producerService = producerService;
            _mapper = mapper;
        }
        [Route("admin/producer")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult Index()
        {
            var model = _producerService.GetAll();
            var result = _mapper.Map<IEnumerable<Producer>, IEnumerable<ProducerViewModel>>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/producer/create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("admin/producer/create")]
        public IActionResult Create(ProducerViewModel vm)
        {
            var result = _mapper.Map<ProducerViewModel, Producer>(vm);
            if (ModelState.IsValid)
            {
                _producerService.Insert(result);
                return RedirectToAction("Index", "Producer");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [Route("admin/producer/edit/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(int id)
        {
            var model = _producerService.GetSingleById(id);
            var result = _mapper.Map<Producer, ProducerViewModel>(model);
            return View(result);
        }

        [HttpPost]
        [Route("admin/producer/update")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(ProducerViewModel vm)
        {
            var result = _mapper.Map<ProducerViewModel, Producer>(vm);
            if (ModelState.IsValid)
            {
                _producerService.Update(result);
                return RedirectToAction("Index", "Producer");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/producer/delete")]
        public IActionResult Delete(int id)
        {
            _producerService.Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [HttpPost]
        public IActionResult Test()
        {
            var model = _producerService.GetAll();
            return View();
        }
    }
}