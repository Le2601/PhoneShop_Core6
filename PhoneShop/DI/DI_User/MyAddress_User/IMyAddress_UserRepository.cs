using PhoneShop.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.MyAddress_User
{
    public interface IMyAddress_UserRepository
    {


        public Task<IEnumerable<MyAddressViewModel>> ListById(int id);

        public Task<MyAddressViewModel> GetById(int id);

        void Create(MyAddressData model);

        void Update(int IsDefault, int id);

        void Delete(int id);



    }
}
