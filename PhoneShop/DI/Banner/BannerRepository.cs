using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PhoneShop.DI.Banner
{
    public class BannerRepository : IBannerRepository
    {

        private readonly ShopPhoneDbContext _context;

        public BannerRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

            public void Create(BannerData model)
            {             
                    var item = new PhoneShop.Models.Banner     
                    {
                        Image = model.Image,
                        Position = model.Position,
                        Content = model.Content,
                        ProductId = model.ProductId,      
                    };
                        _context.Add(item);
                        _context.SaveChanges();


            }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BannerViewModel> GetById(int id)
        {
            var item =await _context.BannerProducts.Where(x=> x.Id == id).FirstOrDefaultAsync();

            if(item == null)
            {
                return null;
            }

            var IVM = new BannerViewModel
            {
                Id = item.Id,
                Image = item.Image,
                Position = item.Position,
                Content = item.Content,
                ProductId = item.ProductId,
            };

            return IVM;


        }

        public async Task<List<BannerViewModel>> GetList()
        {
           var items = await _context.BannerProducts.OrderBy(x => x.Id).Select(x=> new BannerViewModel
           {
               Id = x.Id,
               Image = x.Image,
               Position = x.Position,
               Content = x.Content,
               ProductId = x.ProductId,
           }).ToListAsync();
           return items;
            
        }

        public void Update(BannerData model)
        {
            var item = new PhoneShop.Models.Banner
            {
                Image = model.Image,
                Position = model.Position,
                Content = model.Content,
                ProductId = model.ProductId,
            };
            _context.Update(item);
            _context.SaveChanges();
        }
    }
}
