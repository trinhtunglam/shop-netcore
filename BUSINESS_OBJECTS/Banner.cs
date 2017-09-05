using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("Banners")]
    public class Banner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string LinkImage { get; set; }
        public int CategoryId { get; set; }
        public bool Status { get; set; }
        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { set; get; }

    }
}
