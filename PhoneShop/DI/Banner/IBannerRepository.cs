using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Banner
{
    public interface IBannerRepository
    {
        public Task<List<BannerViewModel>> GetList();

        public Task<BannerViewModel> GetById(int id);

        void  Create(BannerData model);

        void Update(BannerData model);

        

        void Delete(int id);






    }
}
