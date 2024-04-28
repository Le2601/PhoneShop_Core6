using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Evaluate_Product_User
{
    public class Evaluate_ProductRepository : IEvaluate_ProductRepository
    {
        private readonly ShopPhoneDbContext _context;
        public Evaluate_ProductRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public async Task<int> Check_Value(int Id_Product)
        {
            var item = await _context.Evaluate_Products.Where(x => x.ProductId == Id_Product).FirstOrDefaultAsync();

            if(item != null)
            {
                return 1;
            }
            else { return 0; }
        }

        public async Task<Evaluate_ProductViewModel> GetById(int Id_Product)
        {
           var item = await _context.Evaluate_Products.Where(x=> x.ProductId == Id_Product).FirstOrDefaultAsync()!;

            var IVM = new Evaluate_ProductViewModel
            {
                Id = item.Id,
                Purchases = item.Purchases,
                ScoreEvaluation = item.ScoreEvaluation,
            };
            return IVM;
        }

        public async Task<List<Evaluate_ProductViewModel>> GetLists()
        {
            var items = await _context.Evaluate_Products.Select(x => new Evaluate_ProductViewModel
            {
                Id = x.Id,
                Purchases = x.Purchases,
                ScoreEvaluation = x.ScoreEvaluation,
            }).ToListAsync();
            return items;
        }
    }
}
