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

        List<ProductViewModel> GetAllProducts();


        public ProductViewModel GetByIdVM(int id);

        public List<CategoryModelView> GetCategoryList();

       

        public int Create(ProductData model);

        void UpdateProduct(PhoneShop.Models.Product model);

        void DeleteProduct(int id);


        //end product

        //Specifications
        void CreateSpecifications(SpecificationsData model);
        public List<SpecificationsViewModel> GetSpecificationByIdProduct(int id);

        void UpdateSpecificationByIdProduct(int IdProduct, SpecificationsData model);



        //end Specifications

        //image
        void CreateImageProduct(ImageProductData model);

        List<ImageProductData> GetListById(int id);

        void DeleteImageProduct(int id);

        //end image



    }
}
