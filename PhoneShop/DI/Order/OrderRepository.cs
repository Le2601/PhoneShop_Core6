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
    }
}
