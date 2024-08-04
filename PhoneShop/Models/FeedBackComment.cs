using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("FeedBackComment")]
    public class FeedBackComment
    {
        [Key]
        public int Id { get; set; }

        public int RwProductId { get; set; }

        public int AccountIdFeedBack { get; set; }

        public string UserNameFeedBack { get; set; }

        public string Content { get; set; }

        public DateTime Create_At { get; set; } 


        public virtual Product_Review Product_Review { get; set; }



        





    }
}
