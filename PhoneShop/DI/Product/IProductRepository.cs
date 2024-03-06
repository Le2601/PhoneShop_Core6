using System.Collections.Generic;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
namespace PhoneShop.DI.Product
{
    public interface IProductRepository
    {

        //product
        

        List<ProductViewModel> GetAllProducts();


        public ProductViewModel GetByIdVM(int id);

        

       

        public int Create(ProductData model);

        void UpdateProduct(PhoneShop.Models.Product model);

        void DeleteProduct(int id);

        public int CheckTitleCreate(string  title);


        //end product


        //image
       

       
       

        //end image



    }
}
