﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Order
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ShopPhoneDbContext _context;

        public OrderRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public void ComfirmStatus(Models.Order model)
        {
           

            _context.Orders.Update(model);
            _context.SaveChanges();



            

        }

        public void Delete_RepositoryPayment(PaymentResponseViewModel model)
        {
            var iModel = new PhoneShop.Models.PaymentResponse
            {
               Id = model.Id,
                OrderDescription = model.OrderDescription,
                 TransactionId = model.TransactionId,
                 OrderId = model.OrderId,
                 PaymentMethod = model.PaymentMethod,
                 PaymentId = model.PaymentId,
                 Success = model.Success,
                 Token = model.Token,
                 VnPayResponseCode = model.VnPayResponseCode,
            };

            _context.paymentResponses.Remove(iModel);
            _context.SaveChanges();
        }

        public void Delete_Order(OrderData model)
        {
            var item = _context.Orders.Where(x=> x.Id_Order == model.Id_Order).FirstOrDefault();
            //var item_model = new PhoneShop.Models.Order
            //{
            //    Id_Order = model.Id_Order,
            //    Total_Order = model.Total_Order,
            //    Profit = model.Profit,
            //    Order_Date = model.Order_Date,
            //    Order_Status = model.Order_Status,
            //    AccountId = model.AccountId,
            //    PaymentMethod = model.PaymentMethod
            //};

            _context.Orders.Remove(item);
            _context.SaveChanges();
           
        }

        public void Delete_OrderDetails(Order_DetailsViewModel model)
        {
            var iModel = new Order_Details
            {

                OrderId = model.OrderId,
                Id = model.Id,
                Address = model.Address,
                Phone = model.Phone,
                ProductId = model.ProductId,
                Quantity = model.Quantity,
            };

            _context.Order_Details.Remove(iModel);
            _context.SaveChanges();
        }

        public List<OrderViewModel> GetAll()
        {
            

            var items = _context.Orders.OrderBy(x => x.Id_Order).Select(x=> new OrderViewModel
            {
                Id_Order = x.Id_Order,
                Total_Order = x.Total_Order,
                Profit = x.Profit,
                Order_Date = x.Order_Date,
                Order_Status = x.Order_Status,
                AccountId = x.AccountId,
                PaymentMethod = x.PaymentMethod
                
            }).ToList();

           

            return items;
        }

        public Models.Order GetById(string id)
        {
            var x = _context.Orders.Where(x=> x.Id_Order == id).FirstOrDefault();

            

            return x;
        }

        public OrderData GetById_Data(string id)
        {
            var item = _context.Orders.FirstOrDefault(x=> x.Id_Order == id);
            var IData = new OrderData
            {
                Id_Order = item.Id_Order,
                Total_Order = item.Total_Order,
                Profit = item.Profit,
                Order_Date = item.Order_Date,
                Order_Status = item.Order_Status,
                AccountId = item.AccountId,
                PaymentMethod = item.PaymentMethod

            };
            return IData;
        }

        

        public async Task<DeliveryProcessViewModel> GetDeliveryProcessById(string orderId)
        {
           
            var item_DeliveryProcesses = await _context.DeliveryProcesses.Where(x => x.Order_Id == orderId).FirstOrDefaultAsync();
            if (item_DeliveryProcesses == null)
            {

               var IVM_null = new DeliveryProcessViewModel();
                return IVM_null;
            }
            else
            {
                var IVM = new DeliveryProcessViewModel
                {

                    DeliveryAddress = item_DeliveryProcesses.DeliveryAddress,
                    DeliveryStatus = item_DeliveryProcesses.DeliveryStatus,
                    DeliveryDate = item_DeliveryProcesses.DeliveryDate,

                };
                return IVM;
            }

           
        }

        public IEnumerable<Order_DetailsViewModel> GetOrderDetailByOrderId(string orderId)
        {

            var items = _context.Order_Details.Where(x => x.OrderId == orderId).Select(x=> new Order_DetailsViewModel
            {

                OrderId = x.OrderId,
                Id = x.Id,
                Address = x.Address,
                Phone = x.Phone,
                ProductId  = x.ProductId,
                Quantity = x.Quantity,               

            }).ToList();

            return items;



           
        }

        public int GetPaymentMethod(string orderId)
        {
            var item = _context.Orders.FirstOrDefault(x => x.Id_Order == orderId)!.PaymentMethod;

            if(item == null) return 0;

            return item;


        }

        public async Task<PaymentResponseViewModel> GetRepositoryPaymentById(string orderId)
        {
            var GetRepositoryPayment = await _context.paymentResponses.Where(x => x.OrderId == orderId).FirstOrDefaultAsync()!;

            if(GetRepositoryPayment == null)
            {
                var IVM_Null = new PaymentResponseViewModel();
               
                return IVM_Null;
            }

            var IVM = new PaymentResponseViewModel
            {
                Success = GetRepositoryPayment.Success,
                Token = GetRepositoryPayment.Token,
                PaymentMethod = GetRepositoryPayment.PaymentMethod,
                OrderDescription = GetRepositoryPayment.OrderDescription,
                OrderId = GetRepositoryPayment.OrderId,
                VnPayResponseCode = GetRepositoryPayment.VnPayResponseCode,

            };
            return IVM;
        }

        public decimal GetTotal_Order(string orderId)
        {
            var item = _context.Orders.FirstOrDefault(x => x.Id_Order == orderId)!.Total_Order;
            if (item == null) return 0;
            return item;
        }

        public string Get_Address_Order(string orderId)
        {
            var item =  _context.Order_Details.Where(x => x.OrderId == orderId).First().Address;

            if (item == null) return "";
            return item;
        }

        public List<ProductViewModel> ListProduct()
        {
            var item = _context.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
            }).ToList();
            return item;
        }

        //public IEnumerable<Order_DetailsViewModel> GetOrderDetailByOrderId_demo(string orderId)
        //{
        //    var item = _context.Order_Details.Where(x=> x.OrderId == orderId).Select(x=> new Order_DetailsViewModel
        //    {
        //        OrderId = x.OrderId,
        //        productViews = x.productViews.Select(x => new ProductViewModel
        //        {
        //            Id = x.Id,
        //            Title= x.Title,
        //        })
        //    }).ToList();
        //}
    }
}
