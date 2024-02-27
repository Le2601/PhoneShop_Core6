using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace PhoneShop.Models
{
    public class Banner
    {
        [Key]
        public int Id { get; set; }

        public string? Image { get; set; }

        public int Position { get; set; }

        public string? Content { get; set; }

        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }


    }
}
