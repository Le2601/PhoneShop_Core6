
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace PhoneShop.Models
{
    [Table("District")]
    public class District
    {
        [Key]
        public int Id { get; set; }

        public int IdCity { get; set; }

        public string Title { get; set; } = string.Empty;

        public virtual City City { get; set; }
        public virtual ICollection<Ward> Wards { get; set; }

    }
}
