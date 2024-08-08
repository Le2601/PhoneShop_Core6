using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Review_Product")]
    public class Review_Product
    {

        public int Id { get; set; }

        public int Rate { get; set; }

        public string Comments { get; set; }

        public DateTime Create_At { get; set; }

        public int AccountId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual Account Account { get; set; }

        
    }
}
