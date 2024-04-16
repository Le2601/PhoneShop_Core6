using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;

namespace PhoneShop.DI.DI_User.MyAddress_User
{
    public class MyAddress_UserRepository : IMyAddress_UserRepository
    {

        private readonly ShopPhoneDbContext _shopPhoneDbContext;

        public MyAddress_UserRepository(ShopPhoneDbContext shopPhoneDbContext)
        {
            _shopPhoneDbContext = shopPhoneDbContext;
        }
        public  void Create(MyAddressData model)
        {

            var item = new MyAddress
            {
                FullName = model.FullName,
                CityName = model.CityName,
                DistrictName = model.DistrictName,
                WardName = model.WardName,
                Description = model.Description,
                AddressType = model.AddressType,
                IsDefault = model.IsDefault,
                IdAccount = model.IdAccount,
                Phone = model.Phone,
                Email = model.Email,

            };

             _shopPhoneDbContext.MyAddresses.Add(item);
            _shopPhoneDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
           var item = _shopPhoneDbContext.MyAddresses.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _shopPhoneDbContext.Remove(item);
                _shopPhoneDbContext.SaveChanges();
            }
        }

        public async Task<MyAddressViewModel> GetById(int id)
        {
            var x = await _shopPhoneDbContext.MyAddresses.Where(x=> x.Id == id).FirstOrDefaultAsync();

            var IVM = new MyAddressViewModel
            {
                Id = x.Id,
                AddressType = x.AddressType,
                CityName = x.CityName,
                DistrictName = x.DistrictName,
                WardName = x.WardName,
                Description = x.Description,
                Email = x.Email,
                FullName = x.FullName,
                IdAccount = x.IdAccount,
                IsDefault = x.IsDefault,
                Phone = x.Phone,
            };

            return IVM;

        }

        public async Task<IEnumerable<MyAddressViewModel>> ListById(int id)
        {
           var items = await _shopPhoneDbContext.MyAddresses.Where(x => x.IdAccount == id).Select(x=> new MyAddressViewModel
           {
               Id = x.Id,
               AddressType = x.AddressType,
               CityName = x.CityName,
               DistrictName = x.DistrictName,
               WardName = x.WardName,
               Description = x.Description,
               Email = x.Email,
               FullName = x.FullName,
               IdAccount = x.IdAccount,
               IsDefault = x.IsDefault,
               Phone = x.Phone,

           }).ToListAsync();

          return items;
        }

        public void Update(int IsDefault, int id)
        {
            var item = _shopPhoneDbContext.MyAddresses.Where(x => x.Id == id).First();
            item.IsDefault = IsDefault;
            _shopPhoneDbContext.MyAddresses.Update(item);
            _shopPhoneDbContext.SaveChanges();
        }
    }
}
