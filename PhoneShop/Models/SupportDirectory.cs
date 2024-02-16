using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("SupportDirectory")]
    public class SupportDirectory
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        public virtual ICollection<SupportContent> SupportContents { get; set; }
    }
}
