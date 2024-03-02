using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.SupportContentData
{
    public class SupportContent_Repository : ISupportContent_Repository
    {
        private readonly ShopPhoneDbContext _context;
        public SupportContent_Repository(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SupportContentViewModel>> GetAll()
        {
            var items = await _context.Support_Contents.Select(x=> new SupportContentViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias,
                Content = x.Content,
                 IdSpDirectory = x.IdSpDirectory,
                 

            }).ToListAsync();

            return items;
        }

        public async Task<SupportContentViewModel> GetById(int id)
        {
            var x = await _context.Support_Contents.FirstOrDefaultAsync(x => x.Id == id);

            var IVM = new SupportContentViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias,
                Content = x.Content,
                IdSpDirectory = x.IdSpDirectory,
            };

            return IVM;
        }
    }
}
