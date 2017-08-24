using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SERVICES;
using Microsoft.AspNetCore.Identity;
using BUSINESS_OBJECTS;
using SHOP_NETCORE.Models;
using Microsoft.AspNetCore.Authorization;

namespace SHOP_NETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserManagerController : Controller
    {
        private readonly IUserManagerService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserManagerController(IUserManagerService userService
            , UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userService = userService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/role")]
        public IActionResult Index()
        {
            List<ApplicationRoleViewModel> model = new List<ApplicationRoleViewModel>();
            model = _roleManager.Roles.Select(r => new ApplicationRoleViewModel
            {
                RoleName = r.Name,
                Id = r.Id,
                Description = r.Description,
                NumberOfUsers = r.Users.Count
            }).ToList();
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/editrole/{id}")]
        public async Task<IActionResult> EditApplicationRole(string id)
        {
            ApplicationRoleViewModel model = new ApplicationRoleViewModel();
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    model.Id = applicationRole.Id;
                    model.RoleName = applicationRole.Name;
                    model.Description = applicationRole.Description;
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/addrole")]
        public IActionResult AddApplicationRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole(ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool isExist = !String.IsNullOrEmpty(model.Id);
                ApplicationRole applicationRole = isExist ? await _roleManager.FindByIdAsync(model.Id) :
               new ApplicationRole
               {
                   CreateDate = DateTime.UtcNow
               };
                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;
                applicationRole.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleRuslt = await _roleManager.UpdateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole(ApplicationRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole applicationRole = new ApplicationRole
                {
                    CreateDate = DateTime.UtcNow
                };
                applicationRole.Name = model.RoleName;
                applicationRole.Description = model.Description;
                applicationRole.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                IdentityResult roleRuslt = await _roleManager.CreateAsync(applicationRole);
                if (roleRuslt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/deleterole")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(id);
                if (applicationRole != null)
                {
                    IdentityResult roleRuslt = _roleManager.DeleteAsync(applicationRole).Result;
                    if (roleRuslt.Succeeded)
                    {
                        return Json(new
                        {
                            status = true
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            status = false
                        });
                    }
                }
            }
            return View("Index");
        }

        [Route("admin/usermanager/user")]
        [Authorize(Roles = "Admin")]
        public IActionResult UserIndex()
        {
            var lstRole = _roleManager.Roles.Select(r=>new ApplicationRoleViewModel
            {
                Id=r.Id,
                RoleName=r.Name
            }).ToList();
            ViewBag.lstRole = lstRole;
            List<ApplicationUserViewModel> model = new List<ApplicationUserViewModel>();
            model = _userManager.Users.Select(u => new ApplicationUserViewModel
            {
                Id = u.Id,
                UserName=u.UserName,
                Name = u.FullName,
                Email = u.Email
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/adduser")]
        public async Task<IActionResult> AddUser(string modelString)
        {
            ApplicationUserViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUserViewModel>(modelString);
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    FullName = model.Name,
                    UserName = model.UserName,
                    Email = model.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return Json(new
                            {
                                status=true
                            });
                        }
                    }
                }
            }
            return View(model);
        }

        [Route("admin/usermanager/edituser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUserViewModel model = new ApplicationUserViewModel();
            //model.ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
            //{
            //    Text = r.Name,
            //    Value = r.Id
            //}).ToList();

            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser user = await _userManager.FindByIdAsync(id);
                if (user != null)
                {
                    model.Id = user.Id;
                    model.UserName = user.UserName;
                    model.Name = user.FullName;
                    model.Email = user.Email;
                    model.ApplicationRoleId = _roleManager.Roles.Single(r => r.Name == _userManager.GetRolesAsync(user).Result.Single()).Id;
                }
                return Json(new
                {
                    status = true,
                    data = model
                });
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/updateuser")]
        public async Task<IActionResult> UpdateUser(string modelString)
        {
            ApplicationUserViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<ApplicationUserViewModel>(modelString);
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.UserName;
                    user.FullName = model.Name;
                    user.Email = model.Email;
                    string existingRole = _userManager.GetRolesAsync(user).Result.Single();
                    string existingRoleId = _roleManager.Roles.Single(r => r.Name == existingRole).Id;
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        if (existingRoleId != model.ApplicationRoleId)
                        {
                            IdentityResult roleResult = await _userManager.RemoveFromRoleAsync(user, existingRole);
                            if (roleResult.Succeeded)
                            {
                                ApplicationRole applicationRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);
                                if (applicationRole != null)
                                {
                                    IdentityResult newRoleResult = await _userManager.AddToRoleAsync(user, applicationRole.Name);
                                    if (newRoleResult.Succeeded)
                                    {
                                        return Json(new
                                        {
                                            status = true
                                        });
                                    }
                                }
                            }
                        }
                        return Json(new
                        {
                            status = true
                        });
                    }
                }
            }
            return View(model);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("admin/usermanager/deleteuser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                ApplicationUser applicationUser = await _userManager.FindByIdAsync(id);
                if (applicationUser != null)
                {
                    IdentityResult result = await _userManager.DeleteAsync(applicationUser);
                    if (result.Succeeded)
                    {
                        return Json(new
                        {
                            status = true
                        });
                    }
                }
            }
            return View();
        }
    }
}