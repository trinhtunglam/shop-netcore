using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("Languages")]
    public class Languages
    {
        [Key]
        [MaxLength(50)]
        public string Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
