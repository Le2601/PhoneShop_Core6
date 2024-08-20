using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Controllers.Seller;
using PhoneShop.Data;
using PhoneShop.DI.DI_User.Order_User;
using PhoneShop.DI.DI_User.Voucher_User;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Security.Claims;

namespace PhoneShop.Controllers
{
    [Authorize(Roles = "Seller,User")]
    public class UtilitiesController : Controller
    {
        private readonly ShopPhoneDbContext _dbContext;
        private readonly IVoucher_UserRepository _voucher_UserRepository;



        private readonly IOrder_UserRepository _order_userRepository;
        public UtilitiesController(ShopPhoneDbContext shopPhoneDbContext, IVoucher_UserRepository voucher_UserRepository, IOrder_UserRepository order_UserRepository)
        {
            _voucher_UserRepository = voucher_UserRepository;
            _order_userRepository = order_UserRepository;
            _dbContext = shopPhoneDbContext;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Account()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var item = _dbContext.Accounts.Where(x => x.Id == AccountId).FirstOrDefault();

            return View(item);
        }

        public async Task<IActionResult> ListOrder()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            

            var GetOrder = (
                
                    from or in _dbContext.Orders.Where(x=> x.AccountId == AccountId).ToList()
                    join ord in _dbContext.Order_Details.DefaultIfEmpty()
                        .Include(x=> x.Product)
                        .Include(x=> x.Order_ProductPurchasePrices)
                       
                        on or.Id_Order equals ord.OrderId
                    
                    select new Data.Order_DetailsData
                    {
                        ProductId = ord.ProductId,
                        ProductTitle = ord.Product.Title,
                        ProductImage = ord.Product.ImageDefaultName,
                        ProductAlias = ord.Product.Alias,
                        //
                        PurchasePrice_Product = ord.PurchasePrice_Product,
                        Quantity = ord.Quantity,
                        //
                        TotalAmount = ord.Order_ProductPurchasePrices.First().TotalAmount,
                        FinalAmount = (decimal)ord.Order_ProductPurchasePrices.First().FinalAmount!,
                        OrderDate = or.Order_Date,
                        //order
                        Id = ord.Id,
                        Status_OrderDetail = ord.Status_OrderDetail,
                       

                    }

                ).OrderByDescending(x=> x.OrderDate).ToList();
            //chua xan nhan
            ViewBag.OrderComfirm = _dbContext.DeliveryProcesses.Where(x => x.DeliveryStatus == 1).Include(x=> x.Order_Details).ThenInclude(x => x.Product).ToList();

            //chuan bi
            ViewBag.Orderchuanbi = _dbContext.DeliveryProcesses.Where(x => x.DeliveryStatus == 2).Include(x => x.Order_Details).ThenInclude(x => x.Product).ToList();
            //dang giao hang
            ViewBag.Orderdanggiao = _dbContext.DeliveryProcesses.Where(x => x.DeliveryStatus == 3).Include(x => x.Order_Details).ThenInclude(x => x.Product).ToList();
            //da giao hang
            ViewBag.Orderdagiao = _dbContext.DeliveryProcesses.Where(x => x.DeliveryStatus == 4).Include(x => x.Order_Details).ThenInclude(x => x.Product).ToList();
            //huy
            ViewBag.Orderhuy = _dbContext.DeliveryProcesses.Where(x => x.DeliveryStatus == 5).Include(x => x.Order_Details).ThenInclude(x => x.Product).ToList();
            //status order
            ViewBag.StatusOrder = _dbContext.DeliveryProcesses.Include(x => x.Order_Details).ThenInclude(x=> x.Product).ToList();
            return View(GetOrder);
        }


        public IActionResult Voucher()
        {
            //kiem tra da luu ma giam gia chua
            List<VoucherItemModel> CartVoucher = PhoneShop.Extension.SessionExtensions.GetListSessionCartVoucher("CartVoucher", HttpContext);
            VoucherItemViewModel voucherVM = new()
            {
                VoucherItems = CartVoucher
            };

            


            return View(voucherVM);
        }

        public IActionResult MyAddress()
        {

            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var iCity = _dbContext.Cities.ToList();

            var GetMyAddress = _dbContext.MyAddresses.Where(x=> x.IdAccount == AccountId && x.IsDefault == 1).FirstOrDefault()!;

            if(GetMyAddress == null)
            {
                ViewBag.MyAddress = new MyAddress()
                {
                    FullName = "null"
                };
            }
            else
            {
                ViewBag.MyAddress = GetMyAddress;
            }


            ViewBag.AccountInt = AccountId;


            return View(iCity);
        }

        public IActionResult FollowBooth()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var items = (
                   from b in _dbContext.Booth_Information
                   join fl in _dbContext.UserFollows.Where(x=> x.UserID == AccountId) on b.Id equals fl.BoothID 
                   join t in _dbContext.Booth_Trackings on b.Id equals t.BoothId
                   select new Data.BoothData
                   {
                       BoothId = b.Id,
                       BoothName = b.ShopName,
                       QuantityProductBooth = 0,
                       Quantity_Product = t.Quantity_Product,
                       Total_Sold = t.Total_Sold,
                       Followers = t.Followers,
                       Total_Comment = t.Total_Comments


                   }
               ).ToList();


            return View(items);
        }
        public IActionResult GetDistricts(int id)
        {
            var iCity = _dbContext.Districts.Where(x => x.IdCity == id).ToList();

            string arr = "";

            foreach (var i in iCity)
            {
                string v = $"<option value='{i.Id}'>{i.Title}</option>";
                arr += v;
            }




            return Ok(arr);
        }

        public IActionResult GetWards(int id)
        {
            var iWards = _dbContext.Wards.Where(x => x.IdDistrict == id).ToList();

            string arr = "";

            foreach (var i in iWards)
            {
                string v = $"<option value='{i.Id}'>{i.Title}</option>";
                arr += v;
            }




            return Ok(arr);
        }




    }
}
