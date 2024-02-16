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

        public SupportDirectoryViewModel Create(SupportDirectoryData model)
        {

            if(model == null)
            {
                return null;
            }

            var newSpDir = new Models.SupportDirectory {
                    
                Id = model.Id,
                Title  = model.Title,
                Alias = model.Alias,
            
            };

            _context.Support_Directories.Add(newSpDir);

            _context.SaveChanges();

            var createdViewModel = new SupportDirectoryViewModel
            {
                Id = newSpDir.Id,
                Title = newSpDir.Title,
                Alias = newSpDir.Alias
            };
            return createdViewModel;
             
        }

        public List<SupportDirectoryViewModel> GetAll()
        {
            var items = _context.Support_Directories.Select(x=> new SupportDirectoryViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias,
            }).ToList();
            if(items == null)
            {
                return null;
            }

            return items;
        }

        public SupportDirectoryViewModel GetById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
