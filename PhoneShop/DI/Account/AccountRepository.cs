using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Account
{
    public class AccountRepository : IAccountRepository
    {

        private readonly ShopPhoneDbContext _context;

        public AccountRepository(ShopPhoneDbContext contextt)
        {
            _context = contextt;
        }

        public AccountViewModel GetNameAccount(int Id)
        {
            var item = _context.Accounts.Where(x=> x.Id == Id).FirstOrDefault();

            var itemVM = new AccountViewModel
            {

                Id = item.Id,
                FullName = item.FullName

            };

            return itemVM;

        }
    }
}
