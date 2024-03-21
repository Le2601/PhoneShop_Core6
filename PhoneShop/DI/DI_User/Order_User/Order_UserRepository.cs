using PhoneShop.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

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

            };
            _context.Orders.Add(newOrderr);
            _context.SaveChanges();
        }

        public void Create(Order_DetailsData model)
        {
            throw new NotImplementedException();
        }
    }
}
