using PhoneShop.ModelViews;

namespace PhoneShop.DI.Account
{
    public interface IAccountRepository
    {
        public AccountViewModel GetNameAccount(int Id);
    }
}
