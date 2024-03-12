
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using PhoneShop.ModelViews;
namespace PhoneShop.DI.Role
{
    public interface IRoleRepository
    {

        Task<RoleViewModel> GetById(int id);


        Task<IEnumerable<RoleViewModel>> GetAll();

        Task<PhoneShop.Models.Role> Create(PhoneShop.Models.Role model);

        Task<PhoneShop.Models.Role> Delete(int id);

    }
}
