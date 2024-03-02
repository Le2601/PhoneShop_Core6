using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.SupportContentData
{
    public interface ISupportContent_Repository
    {
        public Task<IEnumerable<SupportContentViewModel>> GetAll();

        public Task<SupportContentViewModel> GetById(int id);


    }
}
