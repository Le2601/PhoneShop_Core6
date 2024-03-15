using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.SupportContent
{
    public interface ISupportContentRepository
    {

        public Task<SupportContentViewModel> GetById(int id);
        public Task<IEnumerable<SupportContentViewModel>> GetAll();

        public int CheckId(int id);

        void Create(SupportContentData model);
        void Update(SupportContentData model);

        void Delete(int id);


    }
}
