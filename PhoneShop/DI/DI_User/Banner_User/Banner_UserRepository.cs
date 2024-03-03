using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Banner_User
{
    public class Banner_UserRepository : IBanner_UserRepository
    {

        private readonly ShopPhoneDbContext _context;

        public Banner_UserRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public async Task<List<BannerViewModel>> GetAll()
        {
            var i = await _context.BannerProducts.Select(x => new BannerViewModel
            {
                Id = x.Id,
                Content = x.Content,
                Image = x.Image,
                ProductId = x.ProductId
            }).ToListAsync();

            return i;
        }
    }
}
