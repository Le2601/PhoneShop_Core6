using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class CustomerCareController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public CustomerCareController(ShopPhoneDbContext context)
        {
            _context = context;
        }

        

        public IActionResult Index()
        {
            //check auth cookie and AccountId Session
            int AccountInt = 0;
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                //check auth cookie
                var userPrincipal = HttpContext.User;
                if (userPrincipal.Identity.IsAuthenticated)
                {

                    var GetIDAccount = userPrincipal.FindFirstValue("AccountId");
                    HttpContext.Session.SetString("AccountId", GetIDAccount);


                    AccountInt = int.Parse(GetIDAccount);
                }
            }
            else
            {
                AccountInt = int.Parse(taikhoanID);
            }
            //End check auth cookie and AccountId Session

           
            //lay ra nhung san pham da ban dc 
            var CustomerPurchase = (from p in _context.Products.Where(x => x.Create_Id == AccountInt).ToList()
                                    join od in _context.Order_Details on p.Id equals od.ProductId
                        join o in _context.Orders on od.OrderId equals o.Id_Order
                        join oPrice in _context.Order_ProductPurchasePrices on od.Id equals oPrice.OrderDetail_Id
                        join a in _context.Accounts on o.AccountId equals a.Id
                        select new CustomerPurchase
                        {
                           IdCustomer = a.Id,
                           NameCustomer = a.FullName,
                           EmailCustomer = a.Email,
                           QuantityPurchase = od.Quantity




                        }).ToList();


            var GroupBy_Customer = CustomerPurchase.GroupBy(x => x.IdCustomer).Select(x => new CustomerPurchase
            {
                IdCustomer = x.Key,
                NameCustomer = x.First().NameCustomer,
                EmailCustomer= x.First().EmailCustomer,
                QuantityPurchase = x.Sum(x => x.QuantityPurchase)

            }).OrderByDescending(x=> x.QuantityPurchase).ToList();


            return View(GroupBy_Customer);
        }

        public IActionResult Detail_Customer_Purchase(int Id)
        {
            //check auth cookie and AccountId Session
            
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            int AccountInt = int.Parse(taikhoanID);


            //lay ra nhung san pham da ban dc 
            var CustomerPurchase = (from p in _context.Products.Where(x => x.Create_Id == AccountInt).ToList()
                                    join od in _context.Order_Details on p.Id equals od.ProductId
                                    join o in _context.Orders.Where(x=> x.AccountId == Id) on od.OrderId equals o.Id_Order
                                    join oPrice in _context.Order_ProductPurchasePrices on od.Id equals oPrice.OrderDetail_Id
                                    join a in _context.Accounts on o.AccountId equals a.Id
                                    select new CustomerPurchase
                                    {
                                        ProductId = p.Id,
                                        ProductTitle = p.Title,
                                        QuantityPurchase = od.Quantity




                                    }).ToList();
            return Json(CustomerPurchase);
        }


    }
}
