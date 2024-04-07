using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Ward")]
    public class Ward
    {
        [Key]
        public int Id { get; set; }

        public int IdDistrict { get; set; }

        public virtual District District { get; set; }
        public string Title { get; set; } = string.Empty;




    }
}
