using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Product_User
{
    public interface IProduct_UserRepository
    {

        public Task<List<ProductViewModel>> LatestProducts();

        public Task<List<ImageProductViewModel>> ImageProducts();


        public Task<List<CategoryModelView>> CategoryProducts();


        public Task<List<ProductViewModel>> ProductByCategory(int categoryId);


        //Detail_Product

        public Task<ProductViewModel> ProductById(string? alias, int id);
        public Task<ProductViewModel> ProductById(int id);
        public string GetTitleCategoryId(int CategoryId);
        //Image
        public Task<List<ImageProductViewModel>> GetListImageById(int IdProduct);


        //review
        public Task<List<Product_ReviewModelView>> GetListReviewById(int IdProduct);

        //specifications

        public Task<SpecificationsViewModel> GetSpeciByIdProduct(int IdProduct);

        //partial related product

        public Task<List<ProductViewModel>> GetListRelatedProduct(int IdCategory);

        public void Create_ProductReview(Product_ReviewData model);

        





    }
}
