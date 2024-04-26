using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Evaluate_Product")]
    public class Evaluate_Product
    {
        [Key]
        public int Id { get; set; }

        //lượt mua
        public  int Purchases { get; set; } = 0;

        //đánh giá sản phẩm theo điểm 
        public double ScoreEvaluation { get; set; } = 0;

        public int ProductId { get; set; }

        public Product Product { get; set; } 

    }
}
