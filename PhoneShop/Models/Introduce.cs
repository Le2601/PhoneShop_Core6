using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Models
{
    public class Introduce
    {

        [Key]
        public int Id { get; set; }

        public string Content { get; set; }

        public int UpdatedAt { get; set; }
    }
}
