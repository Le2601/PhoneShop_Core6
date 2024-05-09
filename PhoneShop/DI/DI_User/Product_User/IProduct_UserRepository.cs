using PhoneShop.Data;
using PhoneShop.Models;
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

        //check 

        public int Check_Quantity_Product(List<CartItemModel> item);


        public Task<PhoneShop.Models.Product> ProductById_Model(int id);


        //giam san pham trong kho

        public void Reduced_In_Stock(int Id_Product, int Get_Quantity_Product_Order);


        //xoa sp khi quantity =< 0

        public void Delete_Product_Quantity_Zero(int Id_Product);



    }
}
