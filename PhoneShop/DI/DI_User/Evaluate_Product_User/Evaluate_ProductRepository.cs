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

       

        public void Check_Evaluate_Insert_Db(int Id_Product, int Get_Quantity_Product_Order)
        {
            var Check_Evaluate =  _context.Evaluate_Products.FirstOrDefault(x => x.ProductId == Id_Product);
            if (Check_Evaluate != null)
            {
                if (Get_Quantity_Product_Order >= 2)
                {
                    Check_Evaluate.Purchases += Get_Quantity_Product_Order;
                }
                else
                {
                    Check_Evaluate.Purchases += 1;
                }


                _context.Evaluate_Products.Update(Check_Evaluate);
               
               
            }
            else
            {

                var Add_Evaluate = new Evaluate_Product
                {
                    Purchases = 1,
                    ProductId = Id_Product,
                };
                _context.Evaluate_Products.Add(Add_Evaluate);
                 //_context.SaveChangesAsync();
               
            }

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
