using PhoneShop.Models;
using SQLitePCL;

namespace PhoneShop.Extension.CollaborativeFiltering
{
    public class CollaborativeFilteringService
    {
        public readonly ShopPhoneDbContext _context;
        public CollaborativeFilteringService(ShopPhoneDbContext context) {

            _context = context;
        
        }

    }
}
