using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("SupportContent")]
    public class SupportContent
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        public string Content { get; set; }

        public int IdSpDirectory { get; set; }
    

        public virtual SupportDirectory SupportDirectory { get; set; }
        
    }
}
