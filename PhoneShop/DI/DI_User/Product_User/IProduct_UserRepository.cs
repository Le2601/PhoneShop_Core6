using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Product_User
{
    public interface IProduct_UserRepository
    {

        public Task<List<ProductViewModel>> LatestProducts();

        public Task<List<ProductViewModel>> Selling_Products();

        public Task<List<ProductViewModel>> GetListRelatedProduct(int IdCategory);


        public Task<ProductViewModel> ProductById(string? alias, int id);
        public Task<ProductViewModel> ProductById(int id);



        public Task<List<ProductViewModel>> ProductByCategory(int categoryId);



        public Task<List<ProductViewModel>> GetProduct_RecentPosts();


        public Task<List<ProductViewModel>> Search_Product(string value_search);


       



        //specifications

        public Task<SpecificationsViewModel> GetSpeciByIdProduct(int IdProduct);

        //partial related product


       







    }
}
