using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Introduce
{
    public class IntroduceRepository : IIntroduceRepository
    {

        private readonly ShopPhoneDbContext _context;

        public IntroduceRepository (ShopPhoneDbContext context)
        {
            _context = context;
        }

        public IntroduceViewModel GetById(int id)
        {
            var item = _context.Introduces.Where(x=> x.Id == id).FirstOrDefault()!;

            var iVM = new IntroduceViewModel
            {
                Id = item.Id,
                Content = item.Content,
               
            };

            return iVM;

        }

        public List<IntroduceViewModel> GetIntroduce()
        {
           var item = _context.Introduces.Take(1).Select(x=> new IntroduceViewModel
           {
               Id = x.Id,
               Content = x.Content,
           }).ToList();

            return item;
        }

        public void Update(IntroduceData model)
        {
            var item = _context.Introduces.Where(x=> x.Id == model.Id).FirstOrDefault()!;


            item.Content = model.Content;
               
          

            _context.Introduces.Update(item);
            _context.SaveChanges();

        }
    }
}
