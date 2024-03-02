using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.SupportDirectoryData
{
    public interface ISupportDirectory_Repository
    {
      

        public Task<IEnumerable<SupportDirectoryViewModel>> GetAll();

    }
}
