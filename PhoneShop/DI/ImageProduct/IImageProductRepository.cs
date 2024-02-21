using PhoneShop.Areas.Admin.Data;

namespace PhoneShop.DI.ImageProduct
{
    public interface IImageProductRepository
    {
        public List<ImageProductData> GetListByIdProduct(int IdProduct);


    }
}
