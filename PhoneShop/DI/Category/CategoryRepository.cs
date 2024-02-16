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

namespace PhoneShop.DI.Category
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ShopPhoneDbContext _dbContext;

        //dung de lay duong dan rooot de luu du lieu upload
        //private readonly IWebHostEnvironment _environment;

        public CategoryRepository(ShopPhoneDbContext dbContext)
        {
            _dbContext = dbContext;

          
        }

        public async Task<PhoneShop.Models.Category> GetById(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }


        public async Task<IEnumerable<PhoneShop.Models.Category>> GetAll()
        {
            return await _dbContext.Categories.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<PhoneShop.Models.Category> Create(PhoneShop.Models.Category model)
        {

            if (model == null)
            {

                return model;

            }
            _dbContext.Categories.Add(model);

            await _dbContext.SaveChangesAsync();

            return model;


        }

        public async Task<PhoneShop.Models.Category> Update(PhoneShop.Models.Category model)
        {



            if (model == null)
            {
                return model;
            }

            _dbContext.Categories.Update(model);

            await _dbContext.SaveChangesAsync();

            return model;

        }

        public async Task<PhoneShop.Models.Category> Delete(int id)
        {
            var item = await _dbContext.Categories.FindAsync(id);

            if (item == null)
            {

                return StatusCode(404, "Không tìm thấy");
            }

            _dbContext.Categories.Remove(item);

            await _dbContext.SaveChangesAsync();

            return item;
        }


        public List<CategoryModelView> GetAllDemo()
        {
            var items = _dbContext.Categories.Select(x => new CategoryModelView
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias,
                Image = x.Image,



            }).ToList();

            return items;
        }

        private Models.Category StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
