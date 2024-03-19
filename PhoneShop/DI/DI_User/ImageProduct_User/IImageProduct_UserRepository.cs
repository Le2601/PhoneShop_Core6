using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.ImageProduct_User
{
    public interface IImageProduct_UserRepository
    {
        //Image
        public Task<List<ImageProductViewModel>> GetListImageById(int IdProduct);
        public Task<List<ImageProductViewModel>> ImageProducts();
    }
}
