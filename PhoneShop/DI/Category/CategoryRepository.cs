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

        public async Task<CategoryModelView> GetById(int id)
        {

            var items = await _dbContext.Categories.FindAsync(id);
            if (items == null)
            {
                return null;
            }
          


            var ItemData = new CategoryModelView
            {
                Id = items.Id,
                Title = items.Title!,
                Alias = items.Alias!,
                Image = items.Image!
            };

            return ItemData;



        }

        public async Task<IEnumerable<PhoneShop.Models.Category>> GetAll()
        {
            return await _dbContext.Categories.OrderBy(x => x.Id).ToListAsync();
        }

        public void Create(CategoryData model)
        {

            var item = new PhoneShop.Models.Category
            {
                Id= model.Id,
                Title   =model.Title,
                Alias = model.Alias,
                Image = model.Image,
                
            };
         
            _dbContext.Categories.Add(item);

             _dbContext.SaveChanges();

          


        }

        public void Update(CategoryData model)
        {

            var item = _dbContext.Categories.SingleOrDefault(x => x.Id == model.Id);
            if(item == null)
            {
                //
            }
            item.Title = model.Title;
            item.Alias = model.Alias;
            item.Image = model.Image;


           

             _dbContext.SaveChanges();

           

           

        }

        public void Delete(int id)
        {
            var item =  _dbContext.Categories.Find(id)!;
         

            _dbContext.Categories.Remove(item);

             _dbContext.SaveChanges();

           
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

        private CategoryModelView StatusCode(int v1, string v2)
        {
            throw new NotImplementedException();
        }
    }
}
