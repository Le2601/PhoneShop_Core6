using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DeliveryProcess
{
    public class DeliveryProcessRepository : IDeliveryProcessRepository
    {
        private readonly ShopPhoneDbContext _shopPhoneDbContext;
        public DeliveryProcessRepository(ShopPhoneDbContext shopPhoneDbContext)
        {
            _shopPhoneDbContext = shopPhoneDbContext;
        }

        public void Create(DeliveryProcessData model)
        {
            var Create_Item = new PhoneShop.Models.DeliveryProcess
            {
                DeliveryStatus = model.DeliveryStatus,
                Order_Id = model.Order_Id,
                DeliveryDate = model.DeliveryDate,
                DeliveryAddress = model.DeliveryAddress,

            };

            _shopPhoneDbContext.DeliveryProcesses.Add(Create_Item);
            _shopPhoneDbContext.SaveChanges();
        }

        public async Task<DeliveryProcessData> GetById(string id)
        {
            var Check_DeliveryProcess_Order = await _shopPhoneDbContext.DeliveryProcesses.Where(x => x.Order_Id == id).Select(x=> new DeliveryProcessData
            {
                DeliveryAddress = x.DeliveryAddress,
                DeliveryStatus = x.DeliveryStatus,
                DeliveryDate = x.DeliveryDate,

            }).FirstOrDefaultAsync();


            return Check_DeliveryProcess_Order;

        }

        public void Update(DeliveryProcessData model, string id)
        {
            var Check_DeliveryProcess_Order = _shopPhoneDbContext.DeliveryProcesses.Where(x => x.Order_Id == id).FirstOrDefault()!;

            Check_DeliveryProcess_Order.DeliveryDate = model.DeliveryDate;
            Check_DeliveryProcess_Order.DeliveryStatus = model.DeliveryStatus;
            Check_DeliveryProcess_Order.DeliveryAddress = model.DeliveryAddress;


            _shopPhoneDbContext.DeliveryProcesses.Update(Check_DeliveryProcess_Order);
            _shopPhoneDbContext.SaveChanges();

        }
    }
}
