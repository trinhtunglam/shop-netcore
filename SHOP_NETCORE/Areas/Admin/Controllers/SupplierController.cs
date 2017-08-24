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
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }
        [Route("admin/supplier")]
        public IActionResult Index()
        {
            var model = _supplierService.GetAll();
            var result = _mapper.Map <IEnumerable<Supplier>,IEnumerable<SupplierViewModel>>(model);
            return View(result);
        }


        [Route("admin/supplier/create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("admin/supplier/create")]
        public IActionResult Create(SupplierViewModel vm)
        {
            var result = _mapper.Map<SupplierViewModel,Supplier>(vm);
            if (ModelState.IsValid)
            {
                _supplierService.Insert(result);
                return RedirectToAction("Index", "Supplier");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [Route("admin/supplier/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var model = _supplierService.GetSingleById(id);
            var result = _mapper.Map<Supplier, SupplierViewModel>(model);
            return View(result);
        }

        [HttpPost]
        [Route("admin/supplier/update")]
        public IActionResult Update(SupplierViewModel vm)
        {
            var result = _mapper.Map<SupplierViewModel, Supplier>(vm);
            if (ModelState.IsValid)
            {
                _supplierService.Update(result);
                return RedirectToAction("Index", "Supplier");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [HttpPost]
        [Route("admin/supplier/delete")]
        public IActionResult Delete(int id)
        {
            _supplierService.Delete(id);
            return Json(new
            {
                status = true
            });
        }
    }
}