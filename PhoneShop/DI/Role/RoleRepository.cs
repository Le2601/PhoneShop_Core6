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
using PhoneShop.Areas.Admin.Data;

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


       


       

        private RoleData StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var item = _dbContext.Roles.Find(id)!;
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }

        public void Create(RoleData model)
        {
            var item = new PhoneShop.Models.Role
            {
                RoleName = model.RoleName,
            };

             _dbContext.Roles.Add(item);

             _dbContext.SaveChanges();

        }

        public void Update(RoleData model)
        {
            var item = new PhoneShop.Models.Role { RoleName = model.RoleName,Id= model.Id };


            _dbContext.Update(item);
            _dbContext.SaveChanges();
        }
    }
}
