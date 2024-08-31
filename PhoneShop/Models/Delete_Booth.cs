using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Delete_Booth")]
    public class Delete_Booth
    {

        public int Id { get; set; }

        public int BoothId { get; set; }

        public int AccountId { get; set; }

        public string Content { get; set; }

        public DateTime Create_At { get; set; }

        public bool Status { get; set; }


        public Booth_Information Booth_Information { get; set; }


    }
}
