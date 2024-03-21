using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Order_User
{
    public interface IOrder_UserRepository
    {

        void Create(OrderData model);

        void Create(Order_DetailsData model);

    }
}
