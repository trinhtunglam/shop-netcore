using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SHOP_NETCORE.Models
{
    public class SlideViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Tên ảnh")]
        public string Name { get; set; }
        [Display(Name = "Link ảnh")]
        public string LinkImage { get; set; }
        [Display(Name = "Slide")]
        public bool Status { get; set; }

    }
}
