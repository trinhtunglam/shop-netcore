using BUSINESS_OBJECTS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Configuration
{
    public class UserRoleSeed
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRoleSeed(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async void Seed()
        {
           
            if ((await _roleManager.FindByNameAsync("Admin")) == null)
            {
                await _roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
            }

        }
    }
}
