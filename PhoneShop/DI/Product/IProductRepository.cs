using System.Collections.Generic;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
namespace PhoneShop.DI.Product
{
    public interface IProductRepository
    {

        //product
        List<ProductViewModel> LatestProducts();

        public ProductViewModel GetByIdVM(int id);

       

        public int Create(ProductData model);

        void DeleteProduct(int id);


        //end product


        void CreateSpecifications(SpecificationsData model);

        //image
        void CreateImageProduct(ImageProductData model);

        List<ImageProductData> GetListById(int id);

        void DeleteImageProduct(int id);

        //end image



    }
}
