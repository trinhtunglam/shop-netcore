using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BUSINESS_OBJECTS
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(256)]
        public string FullName { set; get; }
        [MaxLength(256)]
        public string Address { set; get; }
        public DateTime? BirthDay { set; get; }

       
    }
}
