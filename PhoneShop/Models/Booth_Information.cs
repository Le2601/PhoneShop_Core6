using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Booth_Information")]
    public class Booth_Information
    {
        public Booth_Information()
        {
            ShopAddresses = new HashSet<ShopAddress>();



        }
        [Key]
        public int Id { get; set; }

        public string Avatar { get; set; }
        public int Code_Info { get; set; }

        public string ShopName { get; set; }

        public string Email { get; set; }

         public string Phone { get; set; }

         public DateTime Creare_At { get; set; }

        //hoat dong
        public Boolean IsActive { get; set; }

        //kiem duyet
        public Boolean IsApproved { get; set; }

        public int AccountId { get; set; }


        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<ShopAddress> ShopAddresses { get; set; }
        public virtual ICollection<Shipping_Method> Shipping_Methods { get; set; }
        public virtual ICollection<Bank_Account> Bank_Accounts { get; set; }

        public virtual ICollection<Booth_Tracking> Booth_Trackings { get; set; }

        public virtual ICollection<UserFollows> UserFollows { get; set; }

        public virtual ICollection<Voucher> Vouchers { get; set; }

        public virtual ICollection<Delete_Booth> Delete_Booths { get; set; }


    }
}
