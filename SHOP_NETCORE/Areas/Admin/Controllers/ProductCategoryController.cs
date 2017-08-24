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
    public class ProductCategoryController : Controller
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public ProductCategoryController(IProductCategoryService productCategoryService, IMapper mapper)
        {
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [Route("admin/productcategory")]
        public IActionResult Index()
        {
            var model = _productCategoryService.GetAll();
            var result = _mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        [Route("admin/productcategory/create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        [Route("admin/productcategory/create")]
        public IActionResult Create(ProductCategoryViewModel vm)
        {
            var result = _mapper.Map<ProductCategoryViewModel, ProductCategory>(vm);
            if (ModelState.IsValid)
            {
                _productCategoryService.Insert(result);
                return RedirectToAction("Index", "ProductCategory");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [Route("admin/productcategory/edit/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(int id)
        {
            var model = _productCategoryService.GetSingleById(id);
            var result = _mapper.Map<ProductCategory, ProductCategoryViewModel>(model);
            return View(result);
        }

        [HttpPost]
        [Route("admin/productcategory/update")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Update(ProductCategoryViewModel vm)
        {
            var result = _mapper.Map<ProductCategoryViewModel, ProductCategory>(vm);
            if (ModelState.IsValid)
            {
                _productCategoryService.Update(result);
                return RedirectToAction("Index", "ProductCategory");
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/productcategory/delete")]
        public IActionResult Delete(int id)
        {
            _productCategoryService.Delete(id);
            return Json(new
            {
                status = true
            });
        }
    }
}