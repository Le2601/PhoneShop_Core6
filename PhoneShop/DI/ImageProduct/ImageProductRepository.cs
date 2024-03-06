using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.ImageProduct
{
    public class ImageProductRepository : IImageProductRepository
    {
        private readonly ShopPhoneDbContext _context;

        public ImageProductRepository(ShopPhoneDbContext context) {
        
                _context = context;
            
        }

        public void DeleteImage(int id)
        {
            var item = _context.ImageProducts.Find(id);

            if (item != null)
            {
                _context.ImageProducts.Remove(item);
                _context.SaveChanges();
            }

        }
        public void CreateImageProduct(ImageProductData model)
        {
            var AddImages = new PhoneShop.Models.ImageProduct
            {
                ProductId = model.ProductId,
                DataName = model.DataName,
                Create_at = DateTime.Now,
                IsDefault = model.IsDefault

            };

            _context.ImageProducts.Add(AddImages);
            _context.SaveChanges();
        }

        public ImageProductViewModel GetById(int id)
        {
            var item = _context.ImageProducts.FirstOrDefault(x=> x.Id == id);

            var ItemVM = new ImageProductViewModel
            {
                Id = item.Id,
                ProductId = item.ProductId,
                DataName = item.DataName
            };

            return ItemVM;
        }

        public List<ImageProductViewModel> GetListByIdProduct(int? IdProduct)
        {
            var item = _context.ImageProducts.Where(x=> x.ProductId == IdProduct).Select(x=> new ImageProductViewModel
            {
                Id = x.Id,
                ProductId = x.ProductId,
                DataName = x.DataName,
                IsDefault = x.IsDefault,
                Create_at = x.Create_at


            }).ToList();

            return item;
        }

      
    }
}
