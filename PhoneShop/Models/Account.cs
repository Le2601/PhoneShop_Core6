using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace PhoneShop.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Id { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool Active { get; set; }
        public string FullName { get; set; }
        public int? RoleId { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Role Role { get; set; }

        public ICollection<Evaluate_Product> evaluate_Products { get; set; }

        public ICollection<UserFollows> UserFollows { get; set; }

        public ICollection<Review_Product> review_Products { get; set; }

        public ICollection<MyAddress> myAddresses { get; set; }

        public ICollection<Order> Orders { get; set; }


    }
}
