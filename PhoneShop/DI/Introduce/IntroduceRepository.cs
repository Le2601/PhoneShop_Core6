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
            var item = _context.Introduces.Where(x=> x.Id == model.Id).FirstOrDefault();

            var iUpdate = new PhoneShop.Models.Introduce
            {
                Content = model.Content,
                UpdatedAt = DateTime.Now,
            };

            _context.Update(iUpdate);
            _context.SaveChanges();

        }
    }
}
