﻿using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;

namespace PhoneShop.DI.DI_User.Order_User
{
    public class Order_UserRepository : IOrder_UserRepository
    {
        private readonly ShopPhoneDbContext _context;
        public Order_UserRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public void Create(OrderData model)
        {
            var newOrderr = new PhoneShop.Models.Order
            {
                Id_Order = model.Id_Order,
                PaymentMethod = model.PaymentMethod,
                Order_Status = model.Order_Status,
                Order_Date = model.Order_Date,
                Total_Order = model.Total_Order,
                Profit = model.Profit,
                AccountId = model.AccountId,

            };
            _context.Orders.Add(newOrderr);
            _context.SaveChanges();
        }

        public void Create_Order_Detail(Order_DetailsData model)
        {
            var item = new Order_Details
            {
                Order_Name = model.Order_Name,
                Address = model.Address,
                Phone = model.Phone,
                ProductId = model.ProductId,
                OrderId = model.OrderId,
                Quantity = model.Quantity,
                



            };
            _context.Order_Details.Add(item);
            _context.SaveChanges();
        }

        public async Task<List<OrderViewModel>> ListOrder_User(int IdAccount)
        {
            var items = await _context.Orders.Where(x => x.AccountId == IdAccount).Select(x => new OrderViewModel
            {
                Id_Order = x.Id_Order,
                Total_Order = x.Total_Order,
                Order_Date = x.Order_Date,
                PaymentMethod = x.PaymentMethod,
                Order_Status = x.Order_Status,

            }).ToListAsync();

            return items;
        }
    }
}
