using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("UserFollows")]
    public class UserFollows
    {
        public int Id { get; set; }

        public int UserID { get; set; }

        public int BoothID { get; set; }

        public DateTime FollowedAt { get; set; }

        public virtual Account Account { get; set; }    
        public virtual Booth_Information Booth_Information { get; set; }

       

    }
}
