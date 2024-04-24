using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Evaluate_Product")]
    public class Evaluate_Product
    {
        public int Id { get; set; }

        //lượt mua
        public  int Purchases { get; set; }

        //đánh giá sản phẩm theo điểm 
        public  double ScoreEvaluation { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; } 

    }
}
