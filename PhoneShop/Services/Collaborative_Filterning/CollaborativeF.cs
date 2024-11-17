using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Drawing;

namespace PhoneShop.Services.Collaborative_Filterning
{
    public class CollaborativeF : ICollaborativeF
    {

        public readonly ShopPhoneDbContext _context;

        public CollaborativeF(ShopPhoneDbContext context) {
        
            _context = context;
        
        }
        public async Task<List<Product>> GetRecommended(int AccountId)
        {
            var UserProducts = await _context.Evaluate_Products.Where(x => x.AccountId == AccountId)
                .Select(x => x.ProductId).ToListAsync();


            var OtherUsers = await _context.Evaluate_Products.Where(x => x.AccountId != AccountId)
                .ToListAsync();           
            var GroupedUsers = OtherUsers.GroupBy(x => x.AccountId);




            //do tuong dong user voi tung nguoi
            var Point_UserAndIndiUser = new List<(int IndiUser_Id, double Point)>();
            foreach (var group in GroupedUsers)
            {

                var OtherUserId = group.Key; //lay dai dien accountid

                var OtherUserProducts = group.Select(x => x.ProductId).ToList();

                //tinh do tuong dong
                var Intersection = UserProducts.Intersect(OtherUserProducts).Count(); //sp trung nhau => lay so luong

                var Union = UserProducts.Union(OtherUserProducts).Count() ; //ds moi chua sp cua 2 account => lay so luong

                var Point = (double)Intersection / Union;


                if(Point > 0)
                {
                    Point_UserAndIndiUser.Add((OtherUserId, Point));
                }


            }


            var TopUsers =   Point_UserAndIndiUser.OrderByDescending(x => x.Point)
                .Take(5).Select(x => x.IndiUser_Id).ToList();


            var RecommendedProductIds = await _context.Evaluate_Products
                .Where(x=> TopUsers.Contains(x.AccountId) && !UserProducts.Contains(x.ProductId)) //lay ra sp ma nguoi dung tuong tu da mua && loai bo sp nguoi dung da mua
                .Select(x=> x.ProductId)
                .Distinct() // bo trung nhau
                .ToListAsync();

            var RecommendedProducts = await _context.Products.Where(x => RecommendedProductIds.Contains(x.Id)).ToListAsync();

            return RecommendedProducts;

        }
    }
}
