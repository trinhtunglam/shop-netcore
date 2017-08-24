using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        [Route("admin/home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("admin/denied")]
        public IActionResult Denied()
        {
            return View();
        }
    }
}