using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Category_User
{
    public class Category_UserRepository : ICategory_UserRepository
    {
        private readonly ShopPhoneDbContext _context;

        public Category_UserRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public async Task<List<CategoryModelView>> CategoryProducts()
        {
            var items = await _context.Categories.Select(c => new CategoryModelView
            {
                Id = c.Id,
                Alias = c.Alias!,
                Title = c.Title!,
                Image = c.Image!,


            }).ToListAsync();

            return items;

        }
        public string GetTitleCategoryId(int CategoryId)
        {
            var IVM = _context.Categories.Where(x => x.Id == CategoryId).FirstOrDefault()!.Title;



            return IVM!;


        }
    }
}
