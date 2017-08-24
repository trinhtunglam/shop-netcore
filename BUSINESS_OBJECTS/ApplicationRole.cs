using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BUSINESS_OBJECTS
{
    public class ApplicationRole:IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string IpAddress { get; set; }

    }
}
