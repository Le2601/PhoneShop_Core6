using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Order
{
    public interface IOrderRepository
    {

        public List<OrderViewModel> GetAll();

        public Models.Order GetById(string id);

        void ComfirmStatus(Models.Order model);




        //order_details

        public IEnumerable<Order_Details> GetOrderDetailByOrderId(string orderId);


        public List<ProductViewModel> ListProduct();
        

    }
}
