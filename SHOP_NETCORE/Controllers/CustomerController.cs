using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SHOP_NETCORE.Models;
using SERVICES;
using AutoMapper;
using BUSINESS_OBJECTS;
using COMMONS;

namespace SHOP_NETCORE.Controllers
{
    public class CustomerController : Controller
    {
        string key = CommonConstants.CustomerLogin;
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("dangnhap.html")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("dangky.html")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("dangky.html")]
        public IActionResult Register(CustomerViewModel model)
        {
            var checkEmail = _customerService.GetSingleByEmail(model.Email);
            if(checkEmail !=null)
            {
                ModelState.AddModelError("", "Email đã tồn tại");
            }
            else
            {
                var result = _mapper.Map<CustomerViewModel, Customer>(model);
                result.Status = true;

                if (ModelState.IsValid)
                {
                    var customer = _customerService.Add(result);
                    var customerObj = _customerService.GetSingleById(customer.Id);
                    HttpContext.Session.SetObject(key, customerObj);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký không thành công");
                }
            }
            
            return View("Register");
        }

        [HttpPost]
        [Route("dangnhap.html")]
        public IActionResult Login(LoginCustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool checkLogin = _customerService.CheckLogin(model.Email,model.Password);
                if(checkLogin)
                {
                    var customerObj = _customerService.GetSingleByEmail(model.Email);
                    HttpContext.Session.SetObject(key, customerObj);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Sai email hoặc mật khẩu");
                }
            }
            else
            {
                ModelState.AddModelError("", "Đăng nhập không thành công");
            }
            return View("Login");
        }

        [Route("thoat.html")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}