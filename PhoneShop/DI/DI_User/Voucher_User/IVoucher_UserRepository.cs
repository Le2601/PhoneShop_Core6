using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Voucher_User
{
    public interface IVoucher_UserRepository
    {

         List<VoucherViewModel> GetAll();

        public VoucherViewModel GetByCode(string code);

        public VoucherViewModel GetById(int id);

        void Update(VoucherViewModel model);



    }
}
