using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Category_User
{
    public interface ICategory_UserRepository
    {
        public Task<List<CategoryModelView>> CategoryProducts();

        public string GetTitleCategoryId(int CategoryId);

        public Task<string> GetAliasCategoryId(int id);
    }
}
