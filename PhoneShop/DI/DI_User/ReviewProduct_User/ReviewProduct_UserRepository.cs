using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.ReviewProduct_User
{
    public class ReviewProduct_UserRepository : IReviewProduct_UserRepository
    {
        private readonly ShopPhoneDbContext _context;

        public ReviewProduct_UserRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product_ReviewModelView>> GetListReviewById(int IdProduct)
        {
            var ListReview = await _context.ProductQuestions.Where(x => x.ProductId == IdProduct).Select(x => new Product_ReviewModelView
            {

                Id = x.Id,
                Content = x.Content,
                UserName = x.UserName,
                UserEmail = x.UserEmail,
                CreateAt = x.CreateAt,

            }).OrderByDescending(x=> x.CreateAt).ToListAsync();


            return ListReview;


        }

        public void Create_ProductReview(Product_ReviewData model)
        {
            var newReviewProduct = new ProductQuestions
            {

                ProductId = model.ProductId,
                UserName = model.UserName,
                UserEmail = model.UserEmail,
                Content = model.Content,
                CreateAt = model.CreateAt,
                

            };

            _context.ProductQuestions.Add(newReviewProduct);
            _context.SaveChanges();

        }
    }
}
