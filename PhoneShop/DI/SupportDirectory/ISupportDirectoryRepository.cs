using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;
using System.Collections.Generic;


namespace PhoneShop.DI.SupportDirectory
{
    public interface ISupportDirectoryRepository
    {

         List<SupportDirectoryViewModel> GetAll();


        SupportDirectoryViewModel GetById(int id);

        void Create(SupportDirectoryData model);


        void Delete(int id);
        void Update(SupportDirectoryData model);

        public int CheckId(int id);



    }
}
