namespace PhoneShop.Models
{
    public class Order_ProductPurchasePrice
    {

        public int Id { get; set; }

        public int OrderDetail_Id { get; set; }

        public int? VoucherId { get; set; }

        //tong truoc khi giam
        public decimal TotalAmount { get; set; }
        //sau khi giam 
        public decimal? DiscountAmount { get; set; }

        //tong sao khi giam
        public decimal? FinalAmount { get; set; }


        public virtual Order_Details Order_Details { get; set; }




    }
}
