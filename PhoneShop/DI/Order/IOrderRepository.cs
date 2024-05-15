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

        public string Get_Address_Order(string orderId);


        public List<ProductViewModel> ListProduct();


        public decimal GetTotal_Order(string orderId);
        public int GetPaymentMethod(string orderId);



        public Task<PaymentResponseViewModel> GetRepositoryPaymentById(string orderId);


        //DeliveryProcesses tinh trang giao hang 
        public Task<DeliveryProcessViewModel> GetDeliveryProcessById(string orderId);


    }
}
