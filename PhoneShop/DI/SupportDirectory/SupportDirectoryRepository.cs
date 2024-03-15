using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Collections.Generic;
using System.Linq;

namespace PhoneShop.DI.SupportDirectory
{
    public class SupportDirectoryRepository : ISupportDirectoryRepository
    {

        private readonly ShopPhoneDbContext _context;

        public SupportDirectoryRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public int CheckId(int id)
        {
            var ValueCheck = 0;

            var item = _context.Support_Directories.Find(id);
            if(item == null)
            {
                return ValueCheck;
            }
            return ValueCheck = 1;

        }

        public void Create(SupportDirectoryData model)
        {

          

            var newSpDir = new Models.SupportDirectory {
                    
                Id = model.Id,
                Title  = model.Title,
                Alias = model.Alias,
            
            };

            _context.Support_Directories.Add(newSpDir);

            _context.SaveChanges();

           
             
        }

        public void Delete(int id)
        {
            var item = _context.Support_Directories.Find(id)!;

            _context.Support_Directories.Remove(item);
            _context.SaveChanges();

        }

        public List<SupportDirectoryViewModel> GetAll()
        {
            var items = _context.Support_Directories.Select(x=> new SupportDirectoryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias,
            }).ToList();
            

            return items;
        }

        public SupportDirectoryViewModel GetById(int id)
        {
            var item = _context.Support_Directories.FirstOrDefault(x => x.Id == id)!;

            var iVM = new SupportDirectoryViewModel
            {
                Id= item.Id,
                Title = item.Title,
                Alias = item.Alias,
            };

            return iVM;

        }

        public void Update(SupportDirectoryData model)
        {
            var item = _context.Support_Directories.Find(model.Id)!;


            item.Title = model.Title;
            item.Alias = PhoneShop.Helpper.Utilities.SEOUrl(model.Title);

            _context.Support_Directories.Update(item);
            _context.SaveChanges();
            

        }
    }
}
