using PhoneShop.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Product_User
{
    public interface IProduct_UserRepository
    {

        public Task<List<ProductViewModel>> LatestProducts();

        public Task<List<ProductViewModel>> RandomProduct();

        public Task<IEnumerable<ProductViewModel>> ListProduct_Old();



        public Task<List<ProductViewModel>> Get_Search_Product();


        public Task<List<ProductViewModel>> Selling_Products();

        public Task<List<ProductViewModel>> GetListRelatedProduct(int IdCategory);


        public Task<ProductViewModel> ProductById(string? alias, int id);
        public Task<ProductViewModel> ProductById(int id);



        public Task<IEnumerable<ProductViewModel>> ProductByCategory(int categoryId);



        public Task<List<ProductViewModel>> GetProduct_RecentPosts();


        public Task<IEnumerable<ProductViewModel>> Search_Product(string value_search);


       



        //specifications

        public Task<SpecificationsViewModel> GetSpeciByIdProduct(int IdProduct);

        //check 

        public int Check_Quantity_Product(List<CartItemModel> item);


        public Task<PhoneShop.Models.Product> ProductById_Model(int id);


        //giam san pham trong kho

        public void Reduced_In_Stock(int Id_Product, int Get_Quantity_Product_Order);


        //xoa sp khi quantity =< 0

        public void Delete_Product_Quantity_Zero(int Id_Product);


        //san pham ban chay join 2 bang

        public List<PhoneShop.ModelViews.ProductViewModel> GetList_Selling();
       
        public List<PhoneShop.ModelViews.ProductViewModel> ListDiscountProduct();



        //danh sach san pham trong thuoc gian hang


        public List<PhoneShop.ModelViews.ProductViewModel> ListProductByBooth_All(int IdBooth);

        public List<PhoneShop.ModelViews.ProductViewModel> ListProductByBooth_BestSelling(int IdBooth);
        public List<PhoneShop.ModelViews.ProductViewModel> ListProductByBooth_Outstanding(int IdBooth);



        public List<PhoneShop.ModelViews.ProductViewModel> ListProductByBooth_Rating(int IdBooth);

        




    }
}
