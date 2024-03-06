using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.ImageProduct
{
    public interface IImageProductRepository
    {
        public List<ImageProductViewModel> GetListByIdProduct(int? IdProduct);

        public ImageProductViewModel GetById(int id);


        void DeleteImage(int id);


        void CreateImageProduct(ImageProductData model);

    }
}
