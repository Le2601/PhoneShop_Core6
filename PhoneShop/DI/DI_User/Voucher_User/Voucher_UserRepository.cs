using PhoneShop.Data;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.Order_User;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Services;

namespace PhoneShop.DI.DI_User.Voucher_User
{
    public class Voucher_UserRepository : IVoucher_UserRepository
    {
        private readonly ShopPhoneDbContext _dbContext;
        public Voucher_UserRepository(ShopPhoneDbContext dbContext)
        {
            
            _dbContext = dbContext;
           
        }

        public List<VoucherViewModel> GetAll()
        {
            var items = _dbContext.Vouchers.Select(x => new VoucherViewModel {
                Code = x.Code,
                DiscountAmount = x.DiscountAmount,
                DiscountConditions = x.DiscountConditions,
                ExpiryDate = x.ExpiryDate,
                Id = x.Id,
                IsActive = x.IsActive,
                Quantity = x.Quantity,
            }).ToList();

            return items;
        }

        public VoucherViewModel GetByCode(string code)
        {
            var getVoucher = _dbContext.Vouchers.Where(x => x.Code == code).FirstOrDefault()!;

            var item = new VoucherViewModel
            {
                 Code = getVoucher.Code,
                 DiscountAmount = getVoucher.DiscountAmount,
                 DiscountConditions  = getVoucher.DiscountConditions,
                 ExpiryDate = getVoucher.ExpiryDate,
                 Id = getVoucher.Id,
                 IsActive = getVoucher.IsActive,
                 Quantity = getVoucher.Quantity,
                      
            };
            return item;

        }

        public VoucherViewModel GetById(int id)
        {
            var getVoucher = _dbContext.Vouchers.Where(x => x.Id == id).FirstOrDefault()!;
            var item = new VoucherViewModel
            {
                
                Id = getVoucher.Id,
                Quantity = getVoucher.Quantity,
                Code = getVoucher.Code

            };
            return item;
        }

        public void Update(VoucherViewModel model)
        {
            var item = _dbContext.Vouchers.Find(model.Id)!;
            _dbContext.Vouchers.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
