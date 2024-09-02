using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    public class Setting_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public Setting_SellerController(ShopPhoneDbContext shopPhoneDbContext) { 
        
            _context = shopPhoneDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Settings()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var CheckBoothByAccount = _context.Booth_Information.Where(x => x.AccountId == AccountId).FirstOrDefault()!;

            var CheckDelBooth = _context.Delete_Booths.Where(x=> x.BoothId == CheckBoothByAccount.Id && x.AccountId == CheckBoothByAccount.AccountId).FirstOrDefault();


            ViewBag.CheckDelBooth = CheckDelBooth;



            return View(CheckBoothByAccount);
        }

        [HttpPost]
        public IActionResult DeleteBooth(IFormCollection form)
        {

            int BoothId = int.Parse(form["boothid"]);
            int AccountId = int.Parse(form["accountid"]);
            var content = form["content"];


            var CheckBoothByAccount = _context.Booth_Information.Where(x => x.Id == BoothId && x.AccountId == AccountId).FirstOrDefault();

            if(CheckBoothByAccount == null)
            {
                TempData["Del_False"] = "Xóa thất bại, không tồn tại gian hàng";
                return RedirectToAction("Settings");
            }

            var CreateDelBooth = new Delete_Booth
            {
                BoothId = BoothId,
                AccountId = AccountId,
                Content = content,
                Create_At = DateTime.Now,
                Status = false
                
            };

            _context.Delete_Booths.Add(CreateDelBooth);
            _context.SaveChanges();








            TempData["Del_True"] = "Chờ quản trị viên xác nhận xóa gian hàng";

            return RedirectToAction("Settings");
        }

        [HttpPost]
        public IActionResult ReCall_DelBooth(int Id)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session
            var CheckDelBooth = _context.Delete_Booths.Where(x => x.Id == Id && x.AccountId == AccountId).FirstOrDefault();

            if (CheckDelBooth == null)
            {
                
                return Json(new {success = false});
            }

            if(CheckDelBooth.Status == true)
            {
                return Json(new { success = false });

            }

            _context.Delete_Booths.Remove(CheckDelBooth);
            _context.SaveChanges();
            return Json(new { success = true });



           
        }



    }
}
