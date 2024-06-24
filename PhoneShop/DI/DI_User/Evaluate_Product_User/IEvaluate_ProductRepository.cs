using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Evaluate_Product_User
{
    public interface IEvaluate_ProductRepository
    {
        Task<List<Evaluate_ProductViewModel>> GetLists();

        Task<Evaluate_ProductViewModel> GetById(int Id_Product);

        public Task<int> Check_Value(int Id_Product);
            

        //check quantity insert db


        public void Check_Evaluate_Insert_Db(int Id_Product, int Get_Quantity_Product_Order, int AccountId);

    }
}
