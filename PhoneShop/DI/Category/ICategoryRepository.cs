﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using PhoneShop.ModelViews;
namespace PhoneShop.DI.Category
{
    public interface ICategoryRepository
    {


        Task<PhoneShop.Models.Category> GetById(int id);

        Task<IEnumerable<PhoneShop.Models.Category>> GetAll();

        Task<PhoneShop.Models.Category> Create(PhoneShop.Models.Category model);

        Task<PhoneShop.Models.Category> Update(PhoneShop.Models.Category model);


        Task<PhoneShop.Models.Category> Delete(int id);

        //demo modelview nguoi dung
        List<CategoryModelView> GetAllDemo();



    }
}
