using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.Specification
{
    public interface ISpecificationRepository
    {

        //Specifications
        void CreateSpecifications(SpecificationsData model);
        public List<SpecificationsViewModel> GetSpecificationByIdProduct(int id);

        void UpdateSpecificationByIdProduct(int IdProduct, SpecificationsData model);

        public SpecificationsViewModel GetSpecificationByIdProductt(int id);



        //end Specifications
    }
}
