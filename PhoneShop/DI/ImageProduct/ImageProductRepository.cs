using PhoneShop.Areas.Admin.Data;

namespace PhoneShop.DI.ImageProduct
{
    public class ImageProductRepository : IImageProductRepository
    {
        public int DemoAstract(int so)
        {
            var Getnumber = so;
            return Getnumber;
        }

        public string DemoAstract(string chu)
        {
            var GetWrite = chu;
            return GetWrite;
        }

        public List<ImageProductData> GetListByIdProduct(int IdProduct)
        {
            throw new NotImplementedException();
        }

      
    }
}
