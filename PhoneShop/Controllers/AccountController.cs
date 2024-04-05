﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using PhoneShop.ModelViews;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Extension;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.Helpper;
using PhoneShop.Services;
using static System.Net.WebRequestMethods;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace PhoneShop.Controllers
{

    public class AccountController : Controller
    {

        //ramdom ma 6 chu so otp
        private static string GenerateRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999); // Sinh số ngẫu nhiên từ 100000 đến 999999
            return randomNumber.ToString();
        }

        //biet so 0 thanh +84
        private string FormatPhoneNumber(string phoneNumber)
        {
            // Loại bỏ số 0 đầu tiên (nếu có) và thêm "84+" vào đầu số điện thoại
            phoneNumber = phoneNumber.TrimStart('0');
            phoneNumber = "+84" + phoneNumber;

            return phoneNumber;
        }

        //so sanh otp da tao voi voi dung nhap
        private bool CheckOtpValidity(string phoneNumber, string enteredOtp)
        {
           

            // So sánh mã OTP đã gửi đi với mã OTP mà người dùng nhập
            return string.Equals(phoneNumber, enteredOtp);
        }



        private readonly ShopPhoneDbContext _context;

        private readonly SmsService _smsService;
        public AccountController(ShopPhoneDbContext context, SmsService smsService)
        {
            _smsService = smsService;
            _context = context;
        }
        //[Route("Register.html")]
        //public IActionResult Register()
        //{

        //    return View();
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account model)
        {

            var ListCourse =await _context.Accounts.Where(x => x.Email == model.Email).ToListAsync();

            if (!ModelState.IsValid)
            {

                if (ListCourse.Count > 0)
                {
                   
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(model);
                }

                ///xu ly sms otp
                ///

                HttpContext.Session.SetString("Phone", model.Phone);

                string salt = Utilities.GetRandomKey();
                model.RoleId = 3;
                model.Password = (model.Password.Trim()).ToMD5();
               

                var RegisterAccount = new Account
                {
                    
                    Email = model.Email,
                    Password = model.Password,
                    RoleId = model.RoleId,
                    FullName = model.FullName,
                    Phone = model.Phone,
                    Salt = salt


                };

                // Chuyển đổi đối tượng RegisterAccount thành chuỗi JSON
                var registerAccountJson =  JsonConvert.SerializeObject(RegisterAccount);

                // Lưu chuỗi JSON vào session
                HttpContext.Session.SetString("RegisterAccount", registerAccountJson);


                




                //_context.Add(model);
                //await _context.SaveChangesAsync();

                return RedirectToAction("XacThucOTP", "Account");

                //return RedirectToRoute("Login-student");
            }
           
            return View(model);
        }



        public IActionResult XacThucOTP()
        {

            var getPhone = HttpContext.Session.GetString("Phone");

            ////xoa 0 dau them vao +84
            var FormatPhone = FormatPhoneNumber(getPhone);

            ////lay ra otp tao ngau nhien
            string otpramdom = GenerateRandomNumber();

            ViewBag.otpramdom = otpramdom;

            ////gui otp den sms
           /* _smsService.SendOtpSms(FormatPhone, otpramdom);*/ //khi nao can thi mo ra 


            return View();
        }

        [HttpPost]
        public IActionResult XacThucOTP(IFormCollection form)
        {
            
            string xacthucOTP = form["xacthucOTP"];
            string otpramdom = form["otpramdom"];

            var SosanhOTP = CheckOtpValidity(otpramdom, xacthucOTP);

            if(SosanhOTP == true)
            {
                // Lấy chuỗi JSON từ session
                var registerAccountJson = HttpContext.Session.GetString("RegisterAccount");
                if (!string.IsNullOrEmpty(registerAccountJson))
                {
                    // Chuyển đổi chuỗi JSON thành đối tượng RegisterAccountViewModel
                    var registerAccount = JsonConvert.DeserializeObject<RegisterAccountViewModel>(registerAccountJson);                
                    var item = new Account
                    {

                        FullName = registerAccount.FullName,
                        Password = registerAccount.Password,
                        Email = registerAccount.Email,
                        Phone = registerAccount.Phone,
                        RoleId = registerAccount.RoleId,
                        Salt = registerAccount.Salt


                    };
                 
                    _context.Accounts.Add(item);
                     _context.SaveChanges();
                    TempData["Message"] = "Tạo tài khoản thành công!";
                    return RedirectToAction("Index", "Home");
                }

               
            }
            TempData["ErrorMessage"] = "Xác thực OTP không hợp lệ!";
            return RedirectToAction("Index", "Home");
        }





        [AllowAnonymous]
        [Route("Login.html", Name = "Login-student")]

        public IActionResult Login(string returnUrl = null)
        {
            //neu da dang nhap roi thi vao thang trang home
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID != null && !User.IsInRole("Staff"))
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return View();
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Login.html", Name = "Login-student")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
        
            if (model.Email == null)
            {
                ViewBag.Error = "tài khoản không được bỏ trống";
                //return View(model);
                return View(model);
            }

            try
            {


                if (!ModelState.IsValid)
                {
                    Account kh = _context.Accounts.Include(a => a.Role).SingleOrDefault(a => a.Email.ToLower() == model.Email.ToLower().Trim());




                    if (kh == null)
                    {
                        ViewBag.Error = "tài khoản không tồn tại";
                        return View(model);
                    }



                    string pass = (model.Password.Trim()).ToMD5();

                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Mật khẩu sai";
                        return View(model);
                    }
                    if (kh != null && kh.RoleId == 3)
                    {
                        //dang nhap thanh cong

                        //ghi nhan tg dang nhap
                        kh.LastLogin = DateTime.Now;
                        _context.Update(kh);
                        await _context.SaveChangesAsync();

                        var taikhoan = HttpContext.Session.GetString("AccountId");

                        //identity

                        //luu session makh

                        HttpContext.Session.SetString("AccountId", kh.Id.ToString());

                        HttpContext.Session.SetString("AccountName", kh.FullName);
                        //edentity

                        var userClaims = new List<Claim>
                    {

                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("AccountId", kh.Id.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)


                    };

                        var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                        var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                        await HttpContext.SignInAsync(userPrincipal);



                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Tài khoản sinh viên không tồn tại";
                        return View(model);
                    }






                }
            }
            catch
            {
                ViewBag.Error = "Mật khẩu phải hơn 6 ký tự";
                return View(model);
                //return RedirectToAction("Login", "AccountStudents", new { area = "", routeName = "Login-student" });
            }
            ViewBag.Error = "Đăng nhập thất bại";
            return View(model);
            //return RedirectToAction("Login", "AccountStudents", new { area = "", routeName = "Login-student" });




        }

        //logout

        [Route("logout-user.html", Name = "Logout-student")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId");
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public IActionResult Update(IFormCollection form)
        {
            string FullName = form["FullName"];
            string Phone = form["Phone"];
            var Id = form["Id"];


            int AccountInt = int.Parse(Id);



            var iUpdate = _context.Accounts.FirstOrDefault(x=> x.Id == AccountInt)!;
            iUpdate.FullName = FullName;
            iUpdate.Phone = Phone;
            _context.Accounts.Update(iUpdate);
            _context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }
    }
}
