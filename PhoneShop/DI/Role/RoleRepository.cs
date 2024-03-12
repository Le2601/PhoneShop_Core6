using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using PhoneShop.ModelViews;
namespace PhoneShop.DI.Role
{
    public class RoleRepository : IRoleRepository
    {

        private readonly ShopPhoneDbContext _dbContext;

        //dung de lay duong dan rooot de luu du lieu upload
        //private readonly IWebHostEnvironment _environment;

        public RoleRepository(ShopPhoneDbContext dbContext)
        {
            _dbContext = dbContext;


        }


        public async Task<RoleViewModel> GetById(int id)
        {
            var Item = await _dbContext.Roles.FindAsync(id);

            var IVM = new RoleViewModel
            {
                Id = Item.Id,
                RoleName = Item.RoleName
            };

            //if (item == null)
            //{
            //    return NotFoundResult();
            //}

            return IVM;
            
        }

        public async Task<IEnumerable<RoleViewModel>> GetAll()
        {
            var items = await _dbContext.Roles.Select(x=> new RoleViewModel
            {
                Id = x.Id,
                 RoleName = x.RoleName
            }).ToListAsync();

            return items;

        }


        public async Task<PhoneShop.Models.Role> Create(PhoneShop.Models.Role model)
        {
            //if (model == null)
            //{
            //    throw new ArgumentNullException(nameof(model));
            //}

            await _dbContext.Roles.AddAsync(model);

            await _dbContext.SaveChangesAsync();

            return model;

        }


        public async Task<PhoneShop.Models.Role> Delete(int id)
        {

            var item = await _dbContext.Roles.FindAsync(id);

            if (item == null)
            {

                return StatusCode(404, "Không tìm thấy");
            }

            _dbContext.Roles.Remove(item);

            await _dbContext.SaveChangesAsync();

            return item;

        }

        private Models.Role StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
