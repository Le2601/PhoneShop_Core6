using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class Public_MethodController : Controller
    {
        private readonly ShopPhoneDbContext _context;
      
        public Public_MethodController(ShopPhoneDbContext context)
        {
            _context = context;         
        }

        //san pham da ban
        public static List<OrderByUser> ListProduct_Purchase(ShopPhoneDbContext context,int AccountInt)
        {


           
            //dthu tuan
            var StartDate = DateTime.Now.AddDays(-7).Date;
            var EndDate = DateTime.Now.Date;




            var items_Products = context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            //lay ra nhung san pham da ban dc 
            var demo = (from p in items_Products
                        join od in context.Order_Details on p.Id equals od.ProductId
                        join o in context.Orders on od.OrderId equals o.Id_Order
                        join oPrice in context.Order_ProductPurchasePrices on od.Id equals oPrice.OrderDetail_Id
                        select new OrderByUser
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Quantity_Purchase = od.Quantity,
                            Date_Purchase = o.Order_Date,
                            Info_User = o.AccountId,
                            Order_Id = od.OrderId,
                            InputPrice = p.InputPrice,
                            Price = od.PurchasePrice_Product,//p.Discount > 0 ? p.Discount : p.Price
                            Discount = p.Discount,
                            Order_Status = o.Order_Status,
                            Info_Order_Address_Id = od.Id,

                            Total_Order_DetailByProduct = (decimal)oPrice.FinalAmount,//(p.Discount > 0 ? p.Discount : p.Price)
                            ImageDefault = p.ImageDefaultName,
                            Status_OrderDetail = od.Status_OrderDetail,
                            Price_Apply_Voucher = (decimal)oPrice.DiscountAmount,

                            PaymentMethod = o.PaymentMethod



                        }).ToList();


            
            return demo;
        }

        //kiem tra so luong sp ton kho, da ban
        public static List<Check_Product_Purchases> List_Item_Product_Quantity(ShopPhoneDbContext context, int AccountInt)
        {
            var items_Products = context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            var items_WarehousedProducts = context.WarehousedProducts.ToList();
            var Item_Product_Quantity = (from p in items_Products
                                         join e in items_WarehousedProducts
                                         on p.Id equals e.ProductId

                                         select new Check_Product_Purchases
                                         {
                                             Id = p.Id,
                                             Image = p.ImageDefaultName,
                                             Title = p.Title,
                                             Remaining_Product = p.Quantity,
                                             Sold_Product = e.Quantity - p.Quantity,
                                             Input_Quantity = e.Quantity,



                                         }).ToList();


            return Item_Product_Quantity;
        }

        //5 san pham ban chay nhat
        public static List<BestSellers_Product> TopSellersProducts(ShopPhoneDbContext context, List<Models.Product> items_Products)
        {
            var TopSellersProducts = (from p in items_Products
                                      join od in context.Order_Details on p.Id equals od.ProductId
                                      join o in context.Orders on od.OrderId equals o.Id_Order
                                      group new { p, od } by new { p.Id, p.Title, p.InputPrice, p.Price, p.Discount, p.ImageDefaultName } into g
                                      select new BestSellers_Product
                                      {
                                          Id = g.Key.Id,
                                          Title = g.Key.Title,
                                          TotalQuantityPurchased = g.Sum(x => x.od.Quantity),
                                          //InputPrice = g.Key.InputPrice,
                                          //Price = g.Key.Price,
                                          //Discount = g.Key.Discount,
                                          ImageProductDefault = g.Key.ImageDefaultName
                                      })
                   .OrderByDescending(x => x.TotalQuantityPurchased)
                   .Take(2)
                   .ToList();
            return TopSellersProducts;
        }

    }
}
