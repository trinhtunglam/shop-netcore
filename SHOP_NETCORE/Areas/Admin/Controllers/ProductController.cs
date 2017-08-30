using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProducerService _producerService;
        private readonly ISupplierService _supplierService;
        private readonly IProductCategoryService _productCategoryService;
        private IHostingEnvironment _env;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper,
            IProducerService producerService,
            ISupplierService supplierService,
            IProductCategoryService productCategoryService,
            IHostingEnvironment env)
        {
            _productService = productService;
            _producerService = producerService;
            _supplierService = supplierService;
            _productCategoryService = productCategoryService;
            _env = env;
            _mapper = mapper;
        }

        [Route("admin/product")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult Index(int Page=1)
        {
            int pageSize = 10;
            int totalRow = 0;
            var productModel = _productService.GetListProductPaging(Page, pageSize, out totalRow);

            var model = productModel.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ProductCode = x.ProductCode,
                Alias = x.Alias,
                Price = x.Price,
                PromotionPrice = x.PromotionPrice.Value,
                OriginalPrice = x.OriginalPrice.Value,
                Warranty = x.Warranty,
                Quantity = x.Quantity,
                CreateDate = x.CreateDate,
                Images = x.Images,
                Content = x.Content,
                HotFlag = x.HotFlag,
                Status = x.Status,
                ProducerId = x.ProducerId,
                CategoryId = x.CategoryId,
                SupplierId = x.SupplierId
            }).ToList();

            //var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewData["totalItem"] = totalRow;
            ViewData["currentPage"] = Page;
            ViewData["pageSize"] = pageSize;
            //var model = _productService.GetAll();
            //var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return View(model);
        }

        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult Search(string searchString)
        {
            if (searchString != null)
            {
                var model = _productService.GetAll(searchString);
                var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
                return PartialView(result);
            }
            else
            {
                var model = _productService.GetAll();
                var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
                return PartialView(result);
            }

        }

        [Route("admin/product/create")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public IActionResult Create()
        {
            var producer = _producerService.GetAll();
            var productcategory = _productCategoryService.GetAll();
            var supplier = _supplierService.GetAll();
            ViewBag.CategoryId = new SelectList(productcategory, "Id", "Name");
            ViewBag.ProducerId = new SelectList(producer, "Id", "Name");
            ViewBag.SupplierId = new SelectList(supplier, "Id", "Name");
            return View();
        }
        [HttpPost]
        [Route("admin/product/create")]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            var producer = _producerService.GetAll();
            var productcategory = _productCategoryService.GetAll();
            var supplier = _supplierService.GetAll();
            ViewBag.CategoryId = new SelectList(productcategory, "Id", "Name");
            ViewBag.ProducerId = new SelectList(producer, "Id", "Name");
            ViewBag.SupplierId = new SelectList(supplier, "Id", "Name");

            bool checkUser = _productService.CheckUser(vm.ProductCode);

            if (checkUser)
            {
                ModelState.AddModelError("", "Tên mã không được trùng");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var files = HttpContext.Request.Form.Files;
                    foreach (var Image in files)
                    {
                        if (Image != null && Image.Length > 0)
                        {
                            var file = Image;
                            var uploads = Path.Combine(_env.WebRootPath, "images");

                            if (file.Length > 0)
                            {
                                var fileName = ContentDispositionHeaderValue.Parse
                                    (file.ContentDisposition).FileName.Trim('"');
                                System.Console.WriteLine(fileName);
                                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                                {
                                    await file.CopyToAsync(fileStream);
                                    vm.Images = file.FileName;
                                }
                            }
                        }
                    }

                    var result = _mapper.Map<ProductViewModel, Product>(vm);

                    result.CreateDate = DateTime.Now;
                    _productService.Insert(result);
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới thất bại");
                }
            }
            return View("Create");
        }

        [Route("admin/product/edit/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Edit(int id)
        {
            var producer = _producerService.GetAll();
            var productcategory = _productCategoryService.GetAll();
            var supplier = _supplierService.GetAll();

            ViewBag.CategoryId = new SelectList(productcategory, "Id", "Name");
            ViewBag.ProducerId = new SelectList(producer, "Id", "Name");
            ViewBag.SupplierId = new SelectList(supplier, "Id", "Name");

            var model = _productService.GetSingleById(id);
            var result = _mapper.Map<Product, ProductViewModel>(model);
            return View(result);
        }

        [HttpPost]
        [Route("admin/product/update")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(ProductViewModel vm)
        {
            var producer = _producerService.GetAll();
            var productcategory = _productCategoryService.GetAll();
            var supplier = _supplierService.GetAll();

            ViewBag.CategoryId = new SelectList(productcategory, "Id", "Name");
            ViewBag.ProducerId = new SelectList(producer, "Id", "Name");
            ViewBag.SupplierId = new SelectList(supplier, "Id", "Name");


            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        var uploads = Path.Combine(_env.WebRootPath, "images");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse
                                (file.ContentDisposition).FileName.Trim('"');
                            System.Console.WriteLine(fileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                vm.Images = file.FileName;
                            }
                        }
                    }
                }
                var result = _mapper.Map<ProductViewModel, Product>(vm);

                _productService.Update(result);
                return RedirectToAction("Index", "Product");

                //if (vm.Images == null)
                //{
                //    var result = new Product
                //    {
                //        Id=vm.Id,
                //        Name = vm.Name,
                //        ProductCode = vm.ProductCode,
                //        Alias = vm.ProductCode,
                //        Price = vm.Price,
                //        PromotionPrice = vm.PromotionPrice,
                //        OriginalPrice = vm.OriginalPrice,
                //        Warranty = vm.Warranty,
                //        Quantity = vm.Quantity,
                //        Content = vm.Content,
                //        HotFlag = vm.HotFlag,
                //        Status = vm.Status.Value,
                //        ProducerId = vm.ProducerId,
                //        CategoryId = vm.CategoryId,
                //        SupplierId = vm.SupplierId
                //    };
                //    _productService.Update(result);
                //    return RedirectToAction("Index", "Product");
                //}
                //else
                //{
                //    var files = HttpContext.Request.Form.Files;
                //    foreach (var Image in files)
                //    {
                //        if (Image != null && Image.Length > 0)
                //        {
                //            var file = Image;
                //            var uploads = Path.Combine(_env.WebRootPath, "images");

                //            if (file.Length > 0)
                //            {
                //                var fileName = ContentDispositionHeaderValue.Parse
                //                    (file.ContentDisposition).FileName.Trim('"');
                //                System.Console.WriteLine(fileName);
                //                using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                //                {
                //                    await file.CopyToAsync(fileStream);
                //                    vm.Images = file.FileName;
                //                }
                //            }
                //        }
                //    }
                //    var result = _mapper.Map<ProductViewModel, Product>(vm);

                //    _productService.Update(result);
                //    return RedirectToAction("Index", "Product");
                //}
               
            }
            else
            {
                ModelState.AddModelError("", "Thêm mới thất bại");
            }
            return View("Create");
        }

        [HttpPost]
        [Route("admin/product/delete")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return Json(new
            {
                status = true
            });
        }

        [Route("admin/product/getbyname")]
        public JsonResult GetListProductByName(string keyword)
        {
            var model = _productService.GetListProductByName(keyword);
            return Json(new
            {
                data = model
            });
        }

        
        [HttpPost]
        public IActionResult GetListProduct(int Page)
        {
            int pageSize = 10;
            int totalRow = 0;
            var productModel = _productService.GetListProductPaging(Page, pageSize, out totalRow);

            var model = productModel.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Name = x.Name,
                ProductCode = x.ProductCode,
                Alias = x.Alias,
                Price = x.Price,
                PromotionPrice = x.PromotionPrice.Value,
                OriginalPrice = x.OriginalPrice.Value,
                Warranty = x.Warranty,
                Quantity = x.Quantity,
                CreateDate = x.CreateDate,
                Images = x.Images,
                Content = x.Content,
                HotFlag = x.HotFlag,
                Status = x.Status,
                ProducerId = x.ProducerId,
                CategoryId = x.CategoryId,
                SupplierId = x.SupplierId
            }).ToList();

            //var productViewModel = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(productModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);

            ViewData["totalItem"] = totalRow;
            ViewData["currentPage"] = Page;
            ViewData["pageSize"] = pageSize;
            //var model = _productService.GetAll();
            //var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);
            return PartialView("Search",model);
        }

    }
}