using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.ReviewProduct_User
{
    public interface IReviewProduct_UserRepository
    {
        //review
        public Task<List<Product_ReviewModelView>> GetListReviewById(int IdProduct);
        public void Create_ProductReview(Product_ReviewData model);
    }
}
