using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.SupportContent
{
    public class SupportContentRepository : ISupportContentRepository

    {
        private readonly ShopPhoneDbContext _Context;

        public SupportContentRepository(ShopPhoneDbContext context)
        {
            _Context = context;
        }

        public int CheckId(int id)
        {
            var ValueCheck = 0;

            var item = _Context.Support_Contents.Find(id);
            if (item == null)
            {
                return ValueCheck;
            }
            return ValueCheck = 1;
        }

        public void Create(SupportContentData model)
        {
            var item = new PhoneShop.Models.SupportContent
            {
               
                Content = model.Content,
                IdSpDirectory = model.IdSpDirectory,
                Title = model.Title,
                Alias = PhoneShop.Helpper.Utilities.SEOUrl(model.Title)
            };
            _Context.Support_Contents.Add(item);
            _Context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _Context.Support_Contents.Find(id)!;
            _Context.Support_Contents.Remove(item);
            _Context.SaveChanges();
        }

        public async Task<IEnumerable<SupportContentViewModel>> GetAll()
        {
            var items = await _Context.Support_Contents.Select(x=> new SupportContentViewModel
            {
                Id = x.Id,
                Content = x.Content,
                IdSpDirectory = x.IdSpDirectory,
                Title = x.Title,
                Alias = x.Alias
            }).ToListAsync();

            return items;
        }

        public async Task<SupportContentViewModel> GetById(int id)
        {
            var x = await _Context.Support_Contents.FirstOrDefaultAsync(x=> x.Id == id);

            var iVM = new SupportContentViewModel
            {
                Id = x.Id,
                Content = x.Content,
                IdSpDirectory = x.IdSpDirectory,
                Title = x.Title,
                Alias = x.Alias
            };

            return iVM;


        }

        public void Update(SupportContentData model)
        {
            var item = _Context.Support_Contents.Find(model.Id)!;

            item.Id = model.Id;
            item.Content = model.Content;
            item.IdSpDirectory = model.IdSpDirectory;
            item.Title = model.Title;
            item.Alias = model.Alias;

            _Context.Support_Contents.Update(item);
            _Context.SaveChanges();


          
        }
    }
}
