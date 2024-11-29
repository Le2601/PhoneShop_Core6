using PhoneShop.Models;
namespace PhoneShop.Services.Collaborative_Filterning_New
{
    public class RecommendationService
    {

        public readonly ShopPhoneDbContext _context;

        public RecommendationService(ShopPhoneDbContext context)
        {

            _context = context;

        }


        public List<Product> GetRecommendations(int AccountId)
        {
            //danh gia diem tuong duong
            var Similarities = new Dictionary<int, double>();

            var UserRatings = _context.Review_Products.Where(x => x.AccountId == AccountId).ToList();

            //lay ra sp mà nguoi dung co de lai danh gia
             var AllUser = _context.Review_Products.Where(x=> x.AccountId != AccountId).Select(x=> x.AccountId).Distinct().ToList();

            foreach (var OtherId in AllUser)
            {


                if (OtherId == AccountId)
                {
                    continue;
                }


                var OtherUserRatings = _context.Review_Products.Where(x => x.AccountId == OtherId).ToList();

                double Similarity = CosineSimilarity(UserRatings, OtherUserRatings);

                Similarities[OtherId] = Similarity;

            }
                //goi y sp 

                var Recommendations = new Dictionary<int, double>();
                foreach (var SimilarUser in Similarities.Keys.OrderByDescending(x=> Similarities[x]) ) {

                    var SimilarUserRatings = _context.Review_Products.Where(x => x.AccountId == SimilarUser).ToList();




                    foreach (var rating in SimilarUserRatings)
                    {
                        if (!UserRatings.Any(x => x.ProductId == rating.ProductId)){

                            if (!Recommendations.ContainsKey(rating.ProductId))
                            {
                                Recommendations[rating.ProductId] = 0;

                            }

                             Recommendations[rating.ProductId] += Similarities[SimilarUser] * rating.Rate; // loi khi danh gia roi hien thi loi ko ton tai khoa

                         }


                     }


                }

            var data = Recommendations.OrderByDescending(x => x.Value)
           .Select(x => _context.Products.Find(x.Key)).ToList();




            var itemm = data.Select(x => new Models.Product
            {
                Id = x.Id,
                Title = x.Title,
                ImageDefaultName = x.ImageDefaultName,
                Price = x.Price,
                Discount = x.Discount,
                Alias = x.Alias,
                evaluate_Products = x.evaluate_Products,
                Quantity = x.Quantity


            }).ToList();











            return data;
        }
         
        //tinh su tuong duong giua nguoi dung va nguoi dung khac Consine
        private double CosineSimilarity(List<Review_Product> UserRatings, List<Review_Product> OtherUserRatings)
        {

            //tu dien luu id sp da da gia
            var UserRatingDict = UserRatings.ToDictionary(x => x.ProductId, r => r.Rate);

            var OtherUserRatingDict = OtherUserRatings.ToDictionary(x => x.ProductId, x => x.Rate);



            double DotProduct = 0.0; //vo huong
            double UserMagnitude = 0.0; //binh phuong
            double OtherUserMagnitude = 0.0;

            //sp user danh gia
            foreach (var ProductId in UserRatingDict.Keys)
            {
                
                if(OtherUserRatingDict.ContainsKey(ProductId))
                {
                    DotProduct += UserRatingDict[ProductId] * OtherUserRatingDict[ProductId];

                }

                UserMagnitude += Math.Pow(UserRatingDict[ProductId], 2);





            }

            //sp other danh gia
            foreach (var score in OtherUserRatingDict.Values)
            {
                OtherUserMagnitude += Math.Pow(score, 2);
            }

            //ktra do lon = 0 de tranh chia cho 0

            if(UserMagnitude == 0 || OtherUserMagnitude == 0)
            {
                return 0;
            }

            //tinh do tuong dong cosine
            return DotProduct / (Math.Sqrt(UserMagnitude) * Math.Sqrt(OtherUserMagnitude) );

        }






    }
}
