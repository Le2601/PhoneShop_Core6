using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Bank_Account")]
    public class Bank_Account
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; }

        public string CMND { get; set; }

        public string Name_Bank { get; set; }

        public string Bank_Account_Number { get; set; }

        public string Bank_Account_Name { get; set; }


        public bool IsActive { get; set; }


        public int BoothId { get; set; }


        public Booth_Information Booth_Information { get; set; }
    }
}
