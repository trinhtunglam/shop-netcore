using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("Slides")]
    public class Slide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string LinkImage { get; set; }
        public bool Status { get; set; }

    }
}
