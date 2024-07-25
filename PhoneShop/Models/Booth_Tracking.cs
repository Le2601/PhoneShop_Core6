using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Booth_Tracking")]
    public class Booth_Tracking
    {
       
        [Key]
        public int Id { get; set; }

        public int Quantity_Product { get; set; }

        public int Total_Sold { get; set; }

        public int Followers { get; set; }

        public double Point_Evaluation { get; set; }

        public int Total_Comments { get; set; }



        public int BoothId { get; set; }

        public Booth_Information Booth_Information { get; set; }

    }
}
