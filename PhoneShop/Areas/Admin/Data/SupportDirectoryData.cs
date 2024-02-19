using PhoneShop.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Areas.Admin.Data
{
    public class SupportDirectoryData
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Alias { get; set; }

        //public virtual ICollection<SupportContent> SupportContents { get; set; }
    }
}
