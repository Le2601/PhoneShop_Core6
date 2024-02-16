using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;
using System.Collections.Generic;


namespace PhoneShop.DI.SupportDirectory
{
    public interface ISupportDirectoryRepository
    {

         List<SupportDirectoryViewModel> GetAll();


        SupportDirectoryViewModel GetById(int id);

        SupportDirectoryViewModel Create(SupportDirectoryData model);






    }
}
