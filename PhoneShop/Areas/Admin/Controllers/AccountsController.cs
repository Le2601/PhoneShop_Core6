using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
//using QuanLySinhVien.ModelView;
using PhoneShop.Areas.Admin.Models;
using PhoneShop.Helpper;
using PhoneShop.Extension;
using System.Text;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountsController : Controller
    {
        private readonly ShopPhoneDbContext _context;



        public AccountsController(ShopPhoneDbContext context)
        {
            _context = context;

        }



        // GET: Admin/Accounts
        public async Task<IActionResult> Index()
        {

            var ListAccountUser = await _context.Accounts.Where(x => x.RoleId == 3).ToListAsync();

            ViewBag.ListAccountSeller = _context.Accounts.Where(x => x.RoleId == 14).ToList();


            return View(ListAccountUser);
        }



        public IActionResult Create()
        {
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id");


            ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "Id", "RoleName");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account model)
        {
            if (ModelState.IsValid)
            {
                string salt = Utilities.GetRandomKey();

                model.Password = (model.Password.Trim()).ToMD5();





                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", model.RoleId);
            ViewBag.RoleId = new SelectList(_context.Roles.ToList(), "Id", "RoleName");
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", account.RoleId);
            return View(account);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Account model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", model.RoleId);
            return View(model);
        }

        
        public IActionResult ViewBooth(int Id)
        {
            var item = _context.Booth_Information.Where(x => x.AccountId == Id).FirstOrDefault()!;

            return RedirectToAction("Detail_Booth", "Booth", new { Id = item.Id });
           



        }
       

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }


        //login va logout

        // login admin


        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]

        public IActionResult Login(string returnUrl = null)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId_Admin");
            //if (taikhoanID != null && !User.IsInRole("Admin"))
            //{
            //    HttpContext.SignOutAsync();
            //    HttpContext.Session.Remove("AccountId_Admin");
            //    return View();
            //}
            if (taikhoanID != null)
            {
                var Check_Role = _context.Accounts.Where(x => x.Id == int.Parse(taikhoanID)).FirstOrDefault()!;
                if (Check_Role.RoleId != 2)
                {
                    return View();
                }
                else if (Check_Role.RoleId == 2)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Account kh = _context.Accounts.Include(a => a.Role).SingleOrDefault(a => a.Email.ToLower() == model.Email.ToLower().Trim());
                    
                    if (kh == null)
                    {
                        ViewBag.Error = "Tài khoản không tồn tại";
                        return View(model);
                    }
                    string pass = (model.Password.Trim()).ToMD5();

                    if (kh.Password.Trim() != pass)
                    {
                        ViewBag.Error = "Sai mật khâu";
                        return View(model);
                    }


                    //dang nhap thanh cong

                    //ghi nhan tg dang nhap
                    kh.LastLogin = DateTime.Now;
                    _context.Update(kh);
                    await _context.SaveChangesAsync();

                    var taikhoan = HttpContext.Session.GetString("AccountId_Admin");

                    //identity

                    //luu session makh

                    HttpContext.Session.SetString("AccountId_Admin", kh.Id.ToString());

                    //edentity

                    var userClaims_Admin = new List<Claim>
                    {

                        new Claim(ClaimTypes.Name, kh.FullName),
                        new Claim(ClaimTypes.Email, kh.Email),
                        new Claim("AccountId_Admin", kh.Id.ToString()),
                        new Claim("RoleId", kh.RoleId.ToString()),
                        new Claim(ClaimTypes.Role, kh.Role.RoleName)


                    };

                    var grandmaIdentity_Admin = new ClaimsIdentity(userClaims_Admin, "User Identity_Admin");
                    var userPrincipal_Admin = new ClaimsPrincipal(new[] { grandmaIdentity_Admin });
                    await HttpContext.SignInAsync(userPrincipal_Admin);



                    return RedirectToAction("Index", "Home", new { Area = "Admin" });



                }
            }
            catch
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }

            return RedirectToAction("Login", "Accounts", new { Area = "Admin" });




        }


        //logout

        [Route("logout.html", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("AccountId_Admin");
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
            catch
            {
                return RedirectToAction("Login", "Accounts", new { Area = "Admin" });
            }
        }


        public IActionResult ViewAccount(int Id)
        {

            var item = _context.Accounts.Where(x => x.Id == Id).FirstOrDefault();

            ViewBag.ListOrder = _context.Orders.Where(x=> x.AccountId == Id).ToList();

            return View(item);
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {

            var CheckOrder = _context.Orders.Where(x => x.AccountId == id).ToList().Count();

            if(CheckOrder == 0)
            {
                var GetAccount = _context.Accounts.Where(x => x.Id == id).FirstOrDefault()!;

                _context.Accounts.Remove(GetAccount);
                _context.SaveChanges();


                return Json(new { success = true });

            }




            return Json( new { success = false });
        }

    }
}
