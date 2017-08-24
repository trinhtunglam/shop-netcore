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
    public class StorageController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        private readonly IReceiptNoteService _receiptService;
        private readonly IReceiptNoteDetailService _receiptDetailService;
        private readonly IMapper _mapper;
        public StorageController(IProductService productService, IMapper mapper,
            ISupplierService supplierService,
            IReceiptNoteService receiptService,
            IReceiptNoteDetailService receiptDetailService)
        {
            _productService = productService;
            _supplierService = supplierService;
            _receiptDetailService = receiptDetailService;
            _receiptService = receiptService;
            _mapper = mapper;
        }

        [Route("admin/storage")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Index()
        {
            var product= _productService.GetAll();
            var productViewModel = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(product);
            ViewBag.lstProduct = productViewModel;

            var supplier = _supplierService.GetAll();
            var supplierViewModel = _mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierViewModel>>(supplier);
            ViewBag.lstSupplier = supplierViewModel;
            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Input(ReceiptNoteViewModel model,List<ReceiptNoteDetailViewModel> lstVm)
        {
            var receipt = _mapper.Map<ReceiptNoteViewModel, ReceiptNote>(model);
            receipt.Status = false;
            var receiptResult =  _receiptService.Add(receipt);

            Product product;
            foreach (var item in lstVm)
            {
                product = _productService.GetSingleById(item.ProductId);
                product.Quantity += item.Quantity;
                item.ReceiptNodeId = receiptResult.Id;
            }

            var lstVmResult = _mapper.Map<List<ReceiptNoteDetailViewModel>, List<ReceiptNoteDetail>>(lstVm);

            _receiptDetailService.InsertRanger(lstVmResult);

            return View();
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/storage/listreceipt")]
        public IActionResult ListReceipt()
        {
            var receipt = _receiptService.GetAll();
            var receiptViewModel = _mapper.Map<IEnumerable<ReceiptNote>, IEnumerable<ReceiptNoteViewModel>>(receipt);
            return View(receiptViewModel);
        }


        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/storage/receiptnotedetail/{id}")]
        public IActionResult ReceiptNoteDetail(int id)
        {
            var model = _receiptDetailService.GetAllByReceiptId(id);
            var result = _mapper.Map<IEnumerable<ReceiptNoteDetail>, IEnumerable<ReceiptNoteDetailViewModel>>(model);
            return View(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult SelectReceipt(int id)
        {
            var model = _receiptService.GetBySelectId(id);
            var result = _mapper.Map<IEnumerable<ReceiptNote>, IEnumerable<ReceiptNoteViewModel>>(model);
            return PartialView("_ReceiptPartial", result);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult ActiveReceipt(int id)
        {
            var modelId = _receiptService.GetSingleById(id);
            modelId.Status = true;
            _receiptService.Update(modelId);

            var model = _receiptService.GetAll();

            var result = _mapper.Map<IEnumerable<ReceiptNote>, IEnumerable<ReceiptNoteViewModel>>(model);
            return PartialView("_ReceiptPartial", result);
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult SearchReceipt(string searchString)
        {
            var model = _receiptService.GetAll(searchString);
            var result = _mapper.Map<IEnumerable<ReceiptNote>, IEnumerable<ReceiptNoteViewModel>>(model);
            return PartialView("_ReceiptPartial", result);
        }

    }
}