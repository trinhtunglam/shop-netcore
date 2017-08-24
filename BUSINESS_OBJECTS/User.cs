using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BUSINESS_OBJECTS
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(256)]
        public string UserName { get; set; }
        [MaxLength(256)]
        public string PassWord { get; set; }
        [MaxLength(256)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        [MaxLength(256)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string Phone { get; set; }
        [MaxLength(256)]
        public string Question { get; set; }
        public string Reply { get; set; }
        public bool Status { get; set; }
        public int UserGroupId { get; set; }
        [ForeignKey("UserGroupId")]
        public virtual UserGroup UserGroup { get; set; }
        //public virtual IEnumerable<Comment> Comments { set; get; }
        //public virtual IEnumerable<Customer> Customers { set; get; }

    }
}
