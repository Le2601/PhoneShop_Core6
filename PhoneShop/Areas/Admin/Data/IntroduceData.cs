using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Areas.Admin.Data
{
    public class IntroduceData
    {
        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
