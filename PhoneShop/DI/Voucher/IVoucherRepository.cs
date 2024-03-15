using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Voucher
{
    public interface IVoucherRepository
    {

        public Task<IEnumerable<VoucherViewModel>> GetAll();

        public Task<VoucherViewModel> GetById(int id);

        public int CheckId(int id);

        void Create(VoucherData model);

        void Update(VoucherData model);

        void Delete(int id);


    }
}
