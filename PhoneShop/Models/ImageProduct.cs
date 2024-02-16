using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace PhoneShop.Models
{

    [Table("ImageProduct")]
    public class ImageProduct
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

       


        [Required(AllowEmptyStrings = true)]
        public string? DataName { get; set; }

        public DateTime? Create_at { get; set; }

        public bool? IsDefault { get; set; }


        public virtual Product Product { get; set; }




    }
}
