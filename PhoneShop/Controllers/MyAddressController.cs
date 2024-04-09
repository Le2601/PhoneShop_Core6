using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var FullName = form["FullName"];
            var City = form["City"];
            var District = form["District"];
            var Ward = form["Ward"];
            var Description = form["Description"];
            var AddressType = form["AddressType"];
            var IsDefault = form["FullName"];

            MyAddress item = new MyAddress
            {
                FullName = FullName,
                CityName = City,
                DistrictName = District,
                WardName = Ward,
                Description = Description,
                AddressType = int.Parse(AddressType),
                IsDefault = int.Parse(IsDefault),
                IdAccount = AccountInt,

            };
            _shopPhoneDbContext.MyAddresses.Add(item);
            _shopPhoneDbContext.SaveChanges();

            

            return RedirectToAction("Utilities","Home");
        }
    }
}
