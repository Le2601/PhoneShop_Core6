using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("DeliveryProcess")]
    public class DeliveryProcess
    {
        [Key]
        public int Id { get; set; }
     
        public string Order_Id { get; set; }

        public int DeliveryStatus { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string DeliveryAddress { get; set; } = string.Empty;

        public int Order_Detail_Id { get; set; }

        public virtual Order_Details Order_Details {  get; set; }

 


    }
}
