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
using Microsoft.AspNetCore.Identity;

namespace SHOP_NETCORE.Controllers
{
    public class CustomerController : Controller
    {
        string key = CommonConstants.CustomerLogin;
        private readonly ICustomerService _customerService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IMapper _mapper;

        public CustomerController(ICustomerService customerService, IMapper mapper,
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager)
        {
            _customerService = customerService;
            _userManager = userManager;
            _signInManager = signInManager;
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

        [HttpPost]
        public IActionResult ExternalLogin()
        {
            // Request a redirect to the external login provider.
            //var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Customer", new { ReturnUrl = returnUrl });
            //var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            //return Challenge(properties, provider);

            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", Url.Action("ExternalLoginCallback", "Customer"));
            return Challenge(properties, "Facebook");
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
           
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            return View();
           
        }
    }
}