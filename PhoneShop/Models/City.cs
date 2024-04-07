using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("City")]
    public class City
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }


        public virtual ICollection<District> Districts { get; set; }

    }
}
