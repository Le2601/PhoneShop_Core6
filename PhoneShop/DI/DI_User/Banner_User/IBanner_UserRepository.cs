using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Banner_User
{
    public interface IBanner_UserRepository
    {

        public Task<List<BannerViewModel>> GetAll();

    }
}
