using PhoneShop.Models;

namespace PhoneShop.Services.Collaborative_Filterning
{
    public interface ICollaborativeF
    {

        public Task<List<Product>> GetRecommended(int AccountId);

    }
}
