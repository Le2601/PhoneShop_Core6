using PhoneShop.Models;

namespace PhoneShop.ModelViews
{
    public class Order_DetailsViewModel
    {
        public int Id { get; set; }

        public string Order_Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }



        public int ProductId { get; set; }
        public decimal PurchasePrice_Product { get; set; }

        public int Quantity { get; set; }

        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        //public virtual ProductViewModel Product { get; set; }

        public IEnumerable<ProductViewModel> productViews { get; set; }


    }
}
