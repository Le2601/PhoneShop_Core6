
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
namespace PhoneShop.DI.Role
{
    public interface IRoleRepository
    {

        Task<PhoneShop.Models.Role> GetById(int id);


        Task<IEnumerable<PhoneShop.Models.Role>> GetAll();

        Task<PhoneShop.Models.Role> Create(PhoneShop.Models.Role model);

        Task<PhoneShop.Models.Role> Delete(int id);

    }
}
