using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("Tags")]
    public class Tag
    {
        [Key]
        [MaxLength(50)]

        public string ID { set; get; }

        [MaxLength(50)]
        [Required]
        public string Name { set; get; }

        [MaxLength(50)]
        [Required]
        public string Type { set; get; }

        //public virtual IEnumerable<ProductTag> ProductTags { set; get; }
    }
}
