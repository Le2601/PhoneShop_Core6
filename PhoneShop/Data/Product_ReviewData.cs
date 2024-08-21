using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Data
{
    public class Product_ReviewData
    {
        [Key]

        public int Id { get; set; }

        public int ProductId { get; set; }


        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string Content { get; set; }

        public int Rate { get; set; }

        //
        public string TitleProduct { get; set; }

        public string ImageProduct { get; set; }



        public DateTime CreateAt { get; set; }
    }
}
