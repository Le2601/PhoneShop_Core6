using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using static Microsoft.ML.Transforms.Text.LatentDirichletAllocationTransformer.ModelParameters;

namespace PhoneShop.DI.Voucher
{
    public class VoucherRepository : IVoucherRepository
    {

        private readonly ShopPhoneDbContext _context;

        public VoucherRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public int CheckId( int id)
        {
            var ValueCheck = 0;
            var item = _context.Vouchers.Find(id);
            if (item != null)
            {
                return ValueCheck = 1;
            }

            return ValueCheck;
        }

        public void Create(VoucherData model)
        {
            var item = new PhoneShop.Models.Voucher
            {
                Code = model.Code,
                DiscountAmount = model.DiscountAmount,
                DiscountConditions = model.DiscountConditions,
                ExpiryDate = model.ExpiryDate,
                Quantity = model.Quantity,
                IsActive = model.IsActive,
                Description = model.Description,
                Start_At = model.Start_At,
                BoothId = model.BoothId

            };

            _context.Vouchers.Add(item);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Vouchers.Find(id)!;
            _context.Vouchers.Remove(item);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<VoucherViewModel>> GetAll()
        {
            var items =await _context.Vouchers.Select(model => new VoucherViewModel
            {

                Id = model.Id,
                Code = model.Code,
                DiscountAmount = model.DiscountAmount,
                DiscountConditions = model.DiscountConditions,
                ExpiryDate = model.ExpiryDate,
                Quantity = model.Quantity,
                IsActive = model.IsActive,
            }).ToListAsync();

            return items;
        }

        public async Task<VoucherViewModel> GetById(int id)
        {
            var model =await _context.Vouchers.FirstOrDefaultAsync(x=> x.Id == id);

           


            var IVM = new VoucherViewModel
            {
                Id = model.Id,
                Code = model.Code,
                DiscountAmount = model.DiscountAmount,
                DiscountConditions = model.DiscountConditions,
                ExpiryDate = model.ExpiryDate,
                Quantity = model.Quantity,
                IsActive = model.IsActive,
                Description = model.Description
            };

            return IVM;

        }

        public void Update(VoucherData model)
        {
           var item = _context.Vouchers.FirstOrDefault(x => x.Id == model.Id);



            item.Code = model.Code;
            item.DiscountAmount = model.DiscountAmount;
            item.DiscountConditions = model.DiscountConditions;
            item.ExpiryDate = model.ExpiryDate;
            item.Quantity = model.Quantity;
            item.IsActive = model.IsActive;
            item.Description = model.Description;

           

            _context.Vouchers.Update(item);
            _context.SaveChanges();




           

        }
    }
}
