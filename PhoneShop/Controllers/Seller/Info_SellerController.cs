using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Helpper;
using PhoneShop.Models;
using Stripe;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    
    public class Info_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public Info_SellerController(ShopPhoneDbContext context)
        {
            _context = context;
        }
       
        [Route("/kenh-nguoi-ban.html")]
        public IActionResult Info_Seller()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            var Check_Seller = _context.Booth_Information.Where(x=> x.AccountId == AccountId).FirstOrDefault();
            if (Check_Seller != null)
            {
                return RedirectToAction("Index","Home_Seller");
            }  

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IFormCollection form, Microsoft.AspNetCore.Http.IFormFile img)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            if (AccountId == 0)
            {
                return RedirectToAction("Index", "Home");
            }   

            Random random = new Random();

            // Tạo một số ngẫu nhiên từ 0 đến 100.000
            int randomNumber_Id = random.Next(0, 100001);
            //
          
            string ShopName = form["ShopName"];
            string Email = form["Email"];
            string Phone = form["Phone"];
            string avatar = "";

            var DateTimeCreate = DateTime.Now;

            //xu ly hinh anh
            if (img != null)
            {



                string extension = Path.GetExtension(img.FileName);
                string imageName = Utilities.SEOUrl(DateTimeCreate.ToString()) + extension;

                avatar = await Utilities.UploadFile(img, @"AvatarBooth", imageName.ToLower());


            }           

            //
            string FullName = form["FullName"];
            string Address = form["Address"];
            string Address_Detail = form["Address_Detail"];
            //
            string COD = form["COD"];
            string Online_Payment = form["Online_Payment"];

            //check shopname
            var CheckShopName = _context.Booth_Information.Where(x => x.ShopName == ShopName).FirstOrDefault();
            if(CheckShopName != null)
            {
                TempData["CheckShopName"] = "Tên gian hàng đã tồn tại, bạn hãy đặt tên khác!";
                return RedirectToAction("Info_Seller");
            }


            
            //insert Booth_Information 
            var item_Booth_Information = new Booth_Information
            {             
                ShopName = ShopName,
                Email = Email,
                Phone = Phone,
                Creare_At = DateTimeCreate,
                AccountId = AccountId,  
                Code_Info = randomNumber_Id,
                Avatar = avatar
            };
           
            _context.Booth_Information.Add(item_Booth_Information);
            _context.SaveChanges();

            //create booth_tracking
            var Create_BoothTracking = new Booth_Tracking
            {
                BoothId = item_Booth_Information.Id
            };
            _context.Booth_Trackings.Add(Create_BoothTracking);
            _context.SaveChanges();


            var get_Booth_Information = _context.Booth_Information.Where(x=> x.Code_Info == randomNumber_Id).FirstOrDefault()!;

           

            //insert ShopAddress 
            var item_ShopAddress = new ShopAddress
            {
                FullName = FullName,
                Address = Address,
                Address_Detail = Address_Detail,
                BoothId = get_Booth_Information.Id,

            };
            _context.ShopAddress.Add(item_ShopAddress);

            //insert ship method
            var item_ShipMethod = new Shipping_Method
            {
                COD = int.Parse(COD),
                Online_Payment = int.Parse(Online_Payment),
                BoothId = get_Booth_Information.Id,

            };
            _context.ShopShipping_MethodAddress.Add(item_ShipMethod);


            var Change_RoleId_Account = _context.Accounts.Where(x=> x.Id == AccountId).FirstOrDefault();
            if (Change_RoleId_Account == null)
            {
                return RedirectToAction("Index", "Home");

            }
            Change_RoleId_Account.RoleId = 14;
            _context.Accounts.Update(Change_RoleId_Account);

            _context.SaveChanges();

            if(get_Booth_Information != null)
            {
                return RedirectToAction("Index", "Home_Seller");
            }

            return View(form);


            
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAvt(IFormCollection form,Microsoft.AspNetCore.Http.IFormFile avatar)
        {
            
            int IdBooth = int.Parse(form["IdBooth"]);

            var CheckBooth = _context.Booth_Information.Where(x => x.Id == IdBooth).FirstOrDefault()!;

           
                //xoa hinh anh cu
                string pathimg = "/AvatarBooth/" + CheckBooth.Avatar!;
                //xoa hinh anh trong folder
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "AvatarBooth/" + CheckBooth.Avatar);
                if (System.IO.File.Exists(pathFile))
                {
                    // Xóa hình ảnh
                    System.IO.File.Delete(pathFile);

                }
                //end xoa hinh anh cu



                string extension = Path.GetExtension(avatar.FileName);
                string imageName = Utilities.SEOUrl(CheckBooth.Creare_At.ToString()) + extension;

                CheckBooth.Avatar = await Utilities.UploadFile(avatar, @"AvatarBooth", imageName.ToLower());

            _context.Booth_Information.Update(CheckBooth);
            _context.SaveChanges();






            TempData["UpAvt"] = "Cập nhật ảnh đại diện thành công!";

            return RedirectToAction("Index", "Home_Seller");
        }

        [HttpGet]
        public IActionResult Update(int BoothInfo)
        {
            var item = _context.Booth_Information.Where(x => x.Id == BoothInfo).FirstOrDefault();




            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Booth_Information model)
        {

            





            _context.Booth_Information.Update(model);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Cập nhật thành công";
            return View();
        }




        ////
        public IActionResult Update_Address_Seller(int AddressId)
        {
            var item = _context.ShopAddress.Where(x=> x.Id ==  AddressId).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Address_Seller(ShopAddress model)
        {
         
                _context.ShopAddress.Update(model);
                _context.SaveChanges();

                TempData["Success"] = "Cập nhật thành công";
            
            return View();
       
        }

        public IActionResult Update_ShippingMethod_Seller(int ShippingMethodId)
        {
            var item = _context.ShopShipping_MethodAddress.Where(x => x.Id == ShippingMethodId).FirstOrDefault();
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_ShippingMethod_Seller(Models.Shipping_Method model)
        {

           

           

            _context.ShopShipping_MethodAddress.Update(model);


            _context.SaveChanges();

            TempData["Success"] = "Cập nhạt thành công";

            return View();

        }
    }
}
