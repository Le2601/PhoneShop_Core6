﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneShop.Data;
using PhoneShop.DI.DI_User.Evaluate_Product_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.Order_User;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.DI_User.Voucher_User;
using PhoneShop.DI.Product;
using PhoneShop.Extension;
using PhoneShop.Libraries;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Services;
using Stripe;
using Stripe.Issuing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    public class CartController : Controller
    {

       

        private readonly ShopPhoneDbContext _dbContext;
        private readonly IVnPayService _vnPayService;
        private readonly IImageProduct_UserRepository _imageProduct_UserRepository;
        private readonly IOrder_UserRepository _order_UserRepository;
        private readonly IVoucher_UserRepository _voucher_UserRepository;
        private readonly IProduct_UserRepository _productRepository;
        private readonly IEvaluate_ProductRepository _evaluate_ProductRepository;

        private readonly EmailService _emailService;


        public CartController(ShopPhoneDbContext dbContext, IVnPayService vnPayService,IImageProduct_UserRepository imageProduct_UserRepository,
            IOrder_UserRepository order_UserRepository,IVoucher_UserRepository voucher_UserRepository,
            EmailService emailService, IProduct_UserRepository productRepository, IEvaluate_ProductRepository evaluate_ProductRepository)
        {
           
            _voucher_UserRepository = voucher_UserRepository;
            _order_UserRepository = order_UserRepository;
            _imageProduct_UserRepository = imageProduct_UserRepository;
            _dbContext = dbContext;
            _vnPayService = vnPayService;
            _emailService = emailService;
            _productRepository = productRepository;
            _evaluate_ProductRepository = evaluate_ProductRepository;
        }

        private int Check_Quantity_Product(List<CartItemModel> item)
        {

            foreach(var i in  item)
            {

                var IProductById = _dbContext.Products.Where(x=> x.Id == i.ProductId).FirstOrDefault()!;

                if( i.Quantity > IProductById.Quantity )
                {
                    return 0;
                }

               
            }
            return 1;
        }

       


        [Route("/cart.html", Name = "Cart")]
        public async Task<IActionResult> Index()
        {        
            List<CartItemModel> CartItems = PhoneShop.Extension.SessionExtensions.GetListSessionCartItem("Cart", HttpContext);


           



            //kiem tra da luu ma giam gia chua
            List<VoucherItemModel> CartVoucher = PhoneShop.Extension.SessionExtensions.GetListSessionCartVoucher("CartVoucher", HttpContext);
            VoucherItemViewModel voucherVM = new()
            {
                VoucherItems = CartVoucher
            };

            ViewBag.voucherVM = voucherVM;
            decimal _DiscountAmount = 0;
            var result = "";
            var GetDiscountAmount = HttpContext.Session.GetString("DiscountAmount"); //

           


            if (GetDiscountAmount == null)
            {
                _DiscountAmount = 0;
            }
            else
            {

                // Tìm vị trí của dấu chấm
                int dotIndex = GetDiscountAmount.IndexOf('.');

                // Nếu có dấu chấm và có ít nhất một ký tự 0 sau dấu chấm
                if (dotIndex != -1 && GetDiscountAmount.Substring(dotIndex + 1).Contains('0'))
                {
                    // Loại bỏ số 0 từ vị trí sau dấu chấm
                    result = GetDiscountAmount.Substring(0, dotIndex + 1) + GetDiscountAmount.Substring(dotIndex + 1).TrimEnd('0');


                }


                _DiscountAmount = decimal.Parse(result);
            }

            






            CartItemViewModel cartVM = new()
            {
                CartItems = CartItems,
                GrandTotal = CartItems.Sum(x => x.Quantity * x.Price),
                OrderTotal = CartItems.Sum(x => x.Quantity * x.Price) - _DiscountAmount

            };

            ViewBag.GrandTotal = cartVM.GrandTotal;
            ViewBag.OrderTotal = cartVM.OrderTotal;
            ViewBag.imageproduct = await _imageProduct_UserRepository.ImageProducts();

            //partial related product
            //ViewBag.Selling_Products = await _productRepository.GetProduct_RecentPosts();
            return View(cartVM);
        }

        //ap dung voucher cap nhat lai gio hang
        [HttpPost]
        public IActionResult ApplyVoucher(IFormCollection form)
        {
            ////session gio hang
            //List<CartItemModel> CartItems = HttpContext.Session.Get<List<CartItemModel>>("Cart") ?? new List<CartItemModel>(); ///gio hang khong co sp thi tra ve



            string Voucher_code = form["Voucher_code"];

            if(Voucher_code.Length == 0)
            {
                TempData["ValueNull"] = "Mã không được để trống!";
                return RedirectToRoute("Cart");
            }




            //kiem tra voucher co dc nguoi dung luu chua
            List<VoucherItemModel> CartVoucher = PhoneShop.Extension.SessionExtensions.GetListSessionCartVoucher("CartVoucher", HttpContext);

            foreach (var item in CartVoucher)
            {
                if (item.Code == Voucher_code)
                {
                    var getVoucher = _voucher_UserRepository.GetByCode(Voucher_code);

                    //if (getVoucher == null)
                    //{
                    //    TempData["VoucherNull"] = "Mã giảm giá không tồn tại!";
                    //    return RedirectToRoute("Cart");
                    //}

                    if(getVoucher.Quantity <= 0 )
                    {

                        TempData["OutOfDiscount"] = "Mã giảm giá hiện tại đã hết!";
                        return RedirectToRoute("Cart");
                    }
                    if (getVoucher.ExpiryDate < DateTime.Now)
                    {
                        TempData["OutOfDiscount"] = "Mã hết hạn  !";
                        return RedirectToRoute("Cart");
                    }

                    //success

                    var getIdVoucher = getVoucher.Id;

                    HttpContext.Session.Set("getIdVoucher", getIdVoucher);



                    var DiscountAmount = getVoucher.DiscountAmount;
                    HttpContext.Session.Set("DiscountAmount", DiscountAmount);


                    CartVoucher.Remove(item);

                    HttpContext.Session.Set("CartVoucher", CartVoucher);


                    TempData["ApplySuccess"] = "Áp dụng mã giảm giá thành công!";
                    return RedirectToRoute("Cart");

                }
                
            }
          
                TempData["VoucherNull_Cart"] = "Mã giảm giá không tồn tại trong phiên lưu trữ!";
                return RedirectToRoute("Cart");
            


           

        }

        public async Task<IActionResult> Add(int id)
        {


            //lay hinh anh mac dinh sp ra
            //ViewBag.imageproduct = _dbContext.ImageProducts.ToList();

            var item = await _productRepository.ProductById_Model(id);

            List<CartItemModel> Cart = HttpContext.Session.Get<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

          

            CartItemModel cartItems = Cart.Where(x=> x.ProductId == id).FirstOrDefault()!;

            if(cartItems == null) {

                
                Cart.Add(new CartItemModel(item));
            
            }
            else
            {
                cartItems.Quantity += 1;
            }

            HttpContext.Session.Set("Cart", Cart);


            return Json(new { success = true }) ;

            //giu nguyen trang
            //return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            List<CartItemModel> CartItems = PhoneShop.Extension.SessionExtensions.GetListSessionCartItem("Cart", HttpContext);

           if (CartItems != null)
            {
                foreach (var item in CartItems)
                {
                    var itemCart = CartItems.FirstOrDefault(x => x.ProductId == id);
                    if (itemCart != null)
                    {

                        CartItems.Remove(itemCart);

                    }
                    HttpContext.Session.Set("Cart", CartItems);

                    return Json(new { success = true });
                    //return RedirectToRoute("Cart");
                }

               
            }

           
            return Json(new { success = false });
        }

        public IActionResult tru(int id)
        {
            List<CartItemModel> CartItems = HttpContext.Session.Get<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            CartItemModel itemCart = CartItems.FirstOrDefault(x => x.ProductId == id);
            if (itemCart != null)
            {
                --itemCart.Quantity;
            }
            if (itemCart.Quantity == 0)
            {
                CartItems.Remove(itemCart);
            }
            HttpContext.Session.Set("Cart", CartItems);
            return RedirectToRoute("Cart");
        }

        public IActionResult cong(int id)
        {
            List<CartItemModel> CartItems = HttpContext.Session.Get<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            if (Check_Quantity_Product(CartItems) == 0)
            {
                TempData["Check_Quantity_Product"] = "Số lượng mua không thể lớn hơn số lượng trong kho!";
                return RedirectToRoute("Cart");
            }
            CartItemModel itemCart = CartItems.FirstOrDefault(x => x.ProductId == id);
            if (itemCart != null)
            {
                ++itemCart.Quantity;
            }
            
            HttpContext.Session.Set("Cart", CartItems);
            return RedirectToRoute("Cart");
        }

        //checkout
        [Authorize]
        public IActionResult CheckOut()
        {
            List<CartItemModel> CartItems = PhoneShop.Extension.SessionExtensions.GetListSessionCartItem("Cart", HttpContext);///gio hang khong co sp thi tra ve
            if (Check_Quantity_Product(CartItems) == 0)
            {
                TempData["Check_Quantity_Product"] = "Số lượng mua không thể lớn hơn số lượng trong kho!";
                return RedirectToRoute("Cart");
            }
            if (CartItems.Count == 0) {
                ViewBag.msgErorr = "Giỏ hàng rỗng không thể thanh toán!";
                return RedirectToRoute("Cart");

            }

            CartItemViewModel cartVM = new()
            {
                CartItems = CartItems,
                GrandTotal = CartItems.Sum(x => x.Quantity * x.Price)
            };

            ViewBag.GrandTotal = cartVM.GrandTotal;

            ////demo
            //var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            //int AccountInt = int.Parse(taikhoanID);

            //ViewBag.getMyAddress = _dbContext.MyAddresses.Where(x=> x.IdAccount == AccountInt && x.IsDefault == 1).FirstOrDefault();


            return View();
        }

       

        //thanh toan khi nhan hang
        [HttpPost]

        public async Task<IActionResult> SubmitCheckOut(IFormCollection form, PaymentInformationModel model)
        {

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var Get_Quantity_Product_Order = 0;
           

            string Order_Name = "";
            string Address = "";
            string Phone = "";
            string AddressType = "";
            string Description = "";
            string Email = "";

            int OptionAddress = Convert.ToInt32(form["OptionAddress"]);
            if(OptionAddress == 1)
            {
                var IAddressType_Account = _dbContext.MyAddresses.Where(x => x.IdAccount == AccountInt && x.IsDefault == 1).FirstOrDefault()!;
                Order_Name = IAddressType_Account.FullName;
                Address = IAddressType_Account.CityName + IAddressType_Account.DistrictName + IAddressType_Account.WardName;
                Phone = IAddressType_Account.Phone;

                AddressType = (IAddressType_Account.AddressType).ToString();
                Description = IAddressType_Account.Description;
                Email = IAddressType_Account.Email;

            }
            else
            {
                Order_Name = form["Order_Name"];
                Address = form["Address"];
                Phone = form["Phone"];

                AddressType = form["AddressType"];
                Description = form["Description"];
                Email = form["Email"];

            }









            //session gio hang
            List<CartItemModel> CartItems = PhoneShop.Extension.SessionExtensions.GetListSessionCartItem("Cart", HttpContext); ///gio hang khong co sp thi tra ve

            Check_Quantity_Product(CartItems);


            //foreach (var i in CartItems)
            //{
            //    var demoo = i.Quantity;
            //}

            int PaymentMethod = Convert.ToInt32(form["PaymentMethod"]);


            //xu ly decimal
            decimal _DiscountAmount = 0;
            var result = "";
            var GetDiscountAmount = HttpContext.Session.GetString("DiscountAmount"); //
            if (GetDiscountAmount == null)
            {
                _DiscountAmount = 0;
            }
            else
            {

                // Tìm vị trí của dấu chấm
                int dotIndex = GetDiscountAmount.IndexOf('.');

                // Nếu có dấu chấm và có ít nhất một ký tự 0 sau dấu chấm
                if (dotIndex != -1 && GetDiscountAmount.Substring(dotIndex + 1).Contains('0'))
                {
                    // Loại bỏ số 0 từ vị trí sau dấu chấm
                    result = GetDiscountAmount.Substring(0, dotIndex + 1) + GetDiscountAmount.Substring(dotIndex + 1).TrimEnd('0');


                }


                _DiscountAmount = decimal.Parse(result);
            }
            //end xu ly decimal
           
            //lay ra tong so tien
            CartItemViewModel cartVMM = new()
            {
                CartItems = CartItems,
                GrandTotal = CartItems.Sum(x => x.Quantity * x.Price),
                OrderTotal = CartItems.Sum(x => x.Quantity * x.Price) - _DiscountAmount,
                Profit = CartItems.Sum(x=> x.Quantity * x.InputPrice)
            };


            Random random = new Random();
            int randomNumber = random.Next();
            
            var Order_Id = "DH2024" + randomNumber;


            //xu ly thanh toan Vnpay
            if (PaymentMethod == 2) {

                //lu du luw vao order
                var newOrderr = new Data.OrderData
                {
                    Id_Order = Order_Id,
                    PaymentMethod = 2,
                    Order_Status = 0,
                    Order_Date = DateTime.Now,
                    Total_Order = cartVMM.OrderTotal,
                    Profit = cartVMM.GrandTotal -  cartVMM.Profit,
                    AccountId = AccountInt,

                };
                _order_UserRepository.Create(newOrderr);

                //lu du luw vao order_detail

                foreach (var item in CartItems)
                {
                    Get_Quantity_Product_Order = item.Quantity;
                    //var newOrder_Details = new Order_DetailsData
                    //{
                    //    Order_Name = Order_Name,
                    //    Address = Address,
                    //    Phone = Phone,
                    //    ProductId = (int)item.ProductId,
                    //    OrderId = Order_Id,
                    //    Quantity = item.Quantity,
                    //    Description = Description,
                    //    AddressType = AddressType,
                    //    Email = Email,




                    //};

                    //_order_UserRepository.Create_Order_Detail(newOrder_Details);

                    var newOrder_Details = new Order_DetailsData
                    {
                        Order_Name = Order_Name,
                        Address = Address,
                        Phone = Phone,
                        ProductId = (int)item.ProductId,
                        OrderId = Order_Id,
                        Quantity = item.Quantity,
                        Description = Description,
                        AddressType = AddressType,
                        Email = Email,



                    };
                    _order_UserRepository.Create_Order_Detail(newOrder_Details);

                    //kiem tra mua so luong bao nhieu insert dữ liệu vào  Evaluate_Products
                    _evaluate_ProductRepository.Check_Evaluate_Insert_Db((int)item.ProductId, Get_Quantity_Product_Order);
                    //giam san pham trong kho
                    _productRepository.Reduced_In_Stock((int)item.ProductId, Get_Quantity_Product_Order);
                    _dbContext.SaveChanges();

                }

                if (HttpContext.Session.GetString("getIdVoucher") != null)
                {
                    handleVoucher();
                }

                _dbContext.SaveChanges();
                HttpContext.Session.Remove("getIdVoucher");



                model.OrderType = Order_Id;
                model.Amount = (double)cartVMM.OrderTotal;
                model.OrderDescription = "Dat hang online vnpay";
                model.Name = Order_Name;
                var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
               
                return Redirect(url);
            }

            //lay ra tong so tien
            CartItemViewModel cartVM = new()
            {
                CartItems = CartItems,
                GrandTotal = CartItems.Sum(x => x.Quantity * x.Price),
                OrderTotal = CartItems.Sum(x => x.Quantity * x.Price) - _DiscountAmount,
                Profit = CartItems.Sum(x => x.Quantity * x.InputPrice)
            };

           

            //XU LY thanh toan khi nhan hang 

            var newOrder = new OrderData
            {
                Id_Order = Order_Id,
                PaymentMethod = 1, //COD
                Order_Date = DateTime.Now,
                Order_Status = 0, // 0 la trang thai chua xac nhan
                Total_Order = cartVM.OrderTotal,
                Profit = cartVMM.GrandTotal - cartVMM.Profit,
                AccountId = AccountInt,

            };

            _order_UserRepository.Create(newOrder);









            foreach (var item in CartItems)
            {
                Get_Quantity_Product_Order = item.Quantity;


                var newOrder_Details = new Order_DetailsData
                {
                    Order_Name = Order_Name,
                    Address = Address,
                    Phone = Phone,
                    ProductId = (int)item.ProductId,
                    OrderId = Order_Id,
                    Quantity = item.Quantity,
                    Description = Description,
                    AddressType = AddressType,
                    Email = Email,



                };
                _order_UserRepository.Create_Order_Detail(newOrder_Details);

                //demo xu ly dependency
                //kiem tra mua so luong bao nhieu insert dữ liệu vào  Evaluate_Products

                _evaluate_ProductRepository.Check_Evaluate_Insert_Db((int)item.ProductId, Get_Quantity_Product_Order);

                //giam san pham trong kho
                _productRepository.Reduced_In_Stock((int)item.ProductId, Get_Quantity_Product_Order);

                _dbContext.SaveChanges();



            }

            if(HttpContext.Session.GetString("getIdVoucher") != null)
            {
                handleVoucher();
            }
        
           

            _dbContext.SaveChanges();


            HttpContext.Session.Remove("getIdVoucher");

            CartItems.Clear();

            HttpContext.Session.Set("Cart", CartItems);

            //KHONG DUOC XOA
            //send mail 

            //string toEmail = Email;
            //string subject = "Đặt hàng thành công!" + DateTime.Now;

            //double doubleValue = (double)cartVM.OrderTotal;

            //string body = "Đơn hàng đã đặt thành công với thành tiền: " + PhoneShop.Extension.Extension.ToVnd(doubleValue);

            //_emailService.SendEmail(toEmail, subject, body);

            //end send mail



            TempData["OrderSuccess"] = "Đặt hàng thành công!";

            return RedirectToRoute("Cart");
        }

        public void handleVoucher()
        {
            //xu ly voucher

            var getIdVoucher = HttpContext.Session.GetString("getIdVoucher")!;

            int ParseID = int.Parse(getIdVoucher);

            var getById = _voucher_UserRepository.GetById(ParseID);

            if (getById == null)
            {
                //
            }
            getById!.Quantity -= 1;
            _voucher_UserRepository.Update(getById);
            //end xu ly voucher


        }

    }
}
