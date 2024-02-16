using System.Collections.Generic;
using PhoneShop.ModelViews;
namespace PhoneShop.DI.Product
{
    public interface IProductRepository
    {

        List<ProductViewModel> LatestProducts();

    }
}
