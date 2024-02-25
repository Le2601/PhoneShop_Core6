using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Product_User
{
    public interface IProduct_UserRepository
    {

        public Task<List<ProductViewModel>> LatestProducts();

        public Task<List<ImageProductViewModel>> ImageProducts();


        public Task<List<CategoryModelView>> CategoryProducts();


        //Detail_Product

        public Task<ProductViewModel> ProductById(string alias, int id);
        public string GetTitleCategoryId(int CategoryId);

        public Task<ImageProductViewModel> GetListImageById(int IdProduct);











    }
}
