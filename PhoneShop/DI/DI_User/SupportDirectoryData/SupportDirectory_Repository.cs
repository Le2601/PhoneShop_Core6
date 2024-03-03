using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.SupportDirectoryData
{
    public class SupportDirectory_Repository : ISupportDirectory_Repository
    {
        private readonly ShopPhoneDbContext _context;

        public SupportDirectory_Repository(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SupportDirectoryViewModel>> GetAll()

        {
            var items = await _context.Support_Directories.Select(x=> new SupportDirectoryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias,
            }).ToListAsync();

            return items;
        }
    }
}
