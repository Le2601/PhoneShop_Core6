using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.ImageProduct_User
{
    public class ImageProduct_UserRepository : IImageProduct_UserRepository
    {
        private readonly ShopPhoneDbContext _context;

        public ImageProduct_UserRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public async Task<List<ImageProductViewModel>> GetListImageById(int IdProduct)
        {
            var items = await _context.ImageProducts.Where(x => x.ProductId == IdProduct).Select(x => new ImageProductViewModel
            {
                Id = x.Id,
                DataName = x.DataName,
                IsDefault = x.IsDefault,
                ProductId = x.ProductId,
            }).ToListAsync();


            return items;

        }
        public async Task<List<ImageProductViewModel>> ImageProducts()
        {
            var items = await _context.ImageProducts.Select(x => new ImageProductViewModel
            {

                Id = x.Id,
                IsDefault = x.IsDefault,
                ProductId = x.ProductId,
                DataName = x.DataName,



            }).ToListAsync();

            return items;
        }
    }
}
