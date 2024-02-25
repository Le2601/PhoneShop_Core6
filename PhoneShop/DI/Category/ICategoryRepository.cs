
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Areas.Admin.Data;
namespace PhoneShop.DI.Category
{
    public interface ICategoryRepository
    {


        Task<CategoryModelView> GetById(int id);

        Task<IEnumerable<PhoneShop.Models.Category>> GetAll();

        void Create(CategoryData model);

         void Update(CategoryData model);


        void Delete(int id);

        //demo modelview nguoi dung
        List<CategoryModelView> GetAllDemo();



    }
}
