using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("Producers")]
    public class Producer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        public string Infomation { get; set; }
        [MaxLength(256)]
        public string Logo { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string Phone { get; set; }
        [MaxLength(256)]
        public string Fax { get; set; }
        public bool Status { get; set; }
       // public virtual IEnumerable<Product> Products { set; get; }
    }
}
