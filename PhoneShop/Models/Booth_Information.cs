using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Booth_Information")]
    public class Booth_Information
    {
        [Key]

        public int Id { get; set; }

        public string ShopName { get; set; }

        public string Email { get; set; }

         public string Phone { get; set; }

         public DateTime Creare_At { get; set; }

         public int AccountId { get; set; }

         public virtual ICollection<ShopAddress> ShopAddresses { get; set; }
        public virtual ICollection<Shipping_Method> Shipping_Methods { get; set; }
        public virtual ICollection<Bank_Account> Bank_Accounts { get; set; }

    }
}
