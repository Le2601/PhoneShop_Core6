using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DeliveryProcess
{
    public interface IDeliveryProcessRepository
    {

        public Task<DeliveryProcessData> GetById(string id);

        void Update(DeliveryProcessData model, int id);

        void Create(DeliveryProcessData model);
        
        


    }
}
