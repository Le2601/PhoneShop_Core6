
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Areas.Admin.Data;

namespace PhoneShop.DI.Role
{
    public interface IRoleRepository
    {

        Task<RoleViewModel> GetById(int id);


        Task<IEnumerable<RoleViewModel>> GetAll();

        void Create(RoleData model);

        void Delete(int id);

        void Update(RoleData model);

    }
}
