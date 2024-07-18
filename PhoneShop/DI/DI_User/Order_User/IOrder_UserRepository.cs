using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Order_User
{
    public interface IOrder_UserRepository
    {

        void Create(OrderData model);

       public  int Create_Order_Detail(Order_DetailsData model);

        public Task<List<OrderViewModel>> ListOrder_User(int IdAccount);

        //xu ly truong hop thanh toan truc tuyen
        void Create_Order_Payment_Onl(OrderData model);

        void Create_Order_Detai_Payment_Onll(Order_DetailsData model);

        public void SaveChanges();

    }
}
