﻿using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.DI_User.Order_User;
using PhoneShop.DI.DI_User.Voucher_User;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Security.Claims;

namespace PhoneShop.Controllers
{
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

            var item = _dbContext.Accounts.Where(x => x.Id == AccountInt).FirstOrDefault();

            return View(item);
        }

        public async Task<IActionResult> ListOrder()
        {//check auth cookie and AccountId Session
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

            var item = await _order_userRepository.ListOrder_User(AccountInt);
            return View(item);
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

            var iCity = _dbContext.Cities.ToList();

            var GetMyAddress = _dbContext.MyAddresses.Where(x=> x.IdAccount == AccountInt && x.IsDefault == 1).FirstOrDefault()!;

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


            ViewBag.AccountInt = AccountInt;


            return View(iCity);
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
