using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class MyAddressController : Controller
    {

        private readonly ShopPhoneDbContext _shopPhoneDbContext;
        public MyAddressController(ShopPhoneDbContext shopPhoneDbContext)
        {
            _shopPhoneDbContext = shopPhoneDbContext;
        }
        public async Task<IActionResult> Index(int? id)
        {
            int IdAccount = 0;
           
            if(id == null)
            {
                if (TempData.ContainsKey("AccountInt"))
                {
                    IdAccount = (int)TempData["AccountInt"]!;
                    // Sử dụng giá trị accountInt ở đây
                }
            }
            else
            {
                IdAccount = (int)id!;
            }
            var items =await _shopPhoneDbContext.MyAddresses.Where(x => x.IdAccount == IdAccount).ToListAsync();

            return View(items);
        }

        [HttpPost]
        [Authorize]

        public IActionResult Create(IFormCollection form)
        {

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            string FullName = form["FullName"];

            string City = form["City"];
            var TitleCity = _shopPhoneDbContext.Cities.Where(x=> x.Id == int.Parse(City)).First().Title;
            string District = form["District"];
            var TitleDistrict = _shopPhoneDbContext.Districts.Where(x => x.Id == int.Parse(District)).First().Title;
            string Ward = form["Ward"];
            var TitleWard = _shopPhoneDbContext.Wards.Where(x => x.Id == int.Parse(Ward)).First().Title;

            string Description = form["Description"];
            string AddressType = form["AddressType"];
            string IsDefault = form["IsDefault"];

            var item = new MyAddress
            {
                FullName = FullName,
                CityName = TitleCity,
                DistrictName = TitleDistrict,
                WardName = TitleWard,
                Description = Description,
                AddressType = int.Parse(AddressType),
                IsDefault = int.Parse(IsDefault),
                IdAccount = AccountInt,

            };
            _shopPhoneDbContext.MyAddresses.Add(item);
            _shopPhoneDbContext.SaveChanges();

            

            return RedirectToAction("Utilities","Home");
        }

        [HttpPost]
        [Authorize]

        public IActionResult Update(IFormCollection form)
        {

            string IsDefault = form["IsDefault"];
            string Id = form["Id"];

            var item = _shopPhoneDbContext.MyAddresses.Where(x=> x.Id == int.Parse(Id)).First();

            item.IsDefault = int.Parse(IsDefault);

            _shopPhoneDbContext.MyAddresses.Update(item);
            _shopPhoneDbContext.SaveChanges();



            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            TempData["AccountInt"] = AccountInt;
            return RedirectToAction("Index");



        }


    }
}
