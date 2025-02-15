﻿using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Order
{
    public interface IOrderRepository
    {

        public List<OrderViewModel> GetAll();

        public Models.Order GetById(string id);

        public OrderData GetById_Data(string id);
        
        void ComfirmStatus(Models.Order model);


        void Delete_Order(OrderData model);



        //order_details

        public IEnumerable<Order_DetailsViewModel> GetOrderDetailByOrderId(string orderId);

        void Delete_OrderDetails(Order_DetailsViewModel model);

        public string Get_Address_Order(string orderId);


        public List<ProductViewModel> ListProduct();


        public decimal GetTotal_Order(string orderId);
        public int GetPaymentMethod(string orderId);



        public Task<PaymentResponseViewModel> GetRepositoryPaymentById(string orderId);


        //DeliveryProcesses tinh trang giao hang 
        public Task<DeliveryProcessViewModel> GetDeliveryProcessById(string orderId);


        void Delete_RepositoryPayment(PaymentResponseViewModel model);


        //public IEnumerable<Order_DetailsViewModel> GetOrderDetailByOrderId_demo(string orderId);





    }
}
