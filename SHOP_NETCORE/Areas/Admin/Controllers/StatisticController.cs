using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StatisticController : Controller
    {

        [Authorize(Roles = "Admin,Manager")]
        [Route("admin/statistic")]
        public IActionResult Index()
        {
            return View();
        }
    }
}