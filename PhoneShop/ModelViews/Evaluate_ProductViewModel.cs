using PhoneShop.Models;

namespace PhoneShop.ModelViews
{
    public class Evaluate_ProductViewModel
    {              
            public int Id { get; set; }

            //lượt mua
            public int Purchases { get; set; } = 0;

            //đánh giá sản phẩm theo điểm 
            public double ScoreEvaluation { get; set; } = 0;

            public int ProductId { get; set; }

            public ProductViewModel Product { get; set; }
     
    }
}
