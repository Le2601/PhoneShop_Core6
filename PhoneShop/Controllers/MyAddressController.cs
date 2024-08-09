using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Controllers.Seller;
using PhoneShop.Data;
using PhoneShop.DI.DI_User.MyAddress_User;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class MyAddressController : Controller
    {

        private readonly ShopPhoneDbContext _shopPhoneDbContext;
        private readonly IMyAddress_UserRepository _userRepository;
        public MyAddressController(ShopPhoneDbContext shopPhoneDbContext, IMyAddress_UserRepository myAddress_UserRepository)
        {
            _userRepository = myAddress_UserRepository;
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
                    // Sử dụng giá trị accountInt ở đâyf
                }
            }
            else
            {
                IdAccount = (int)id!;
            }
             var items = await _userRepository.ListById(IdAccount);

            return View(items);
        }

        [HttpPost]
        [Authorize]

        public IActionResult Create(IFormCollection form)
        {

            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            string FullName = form["FullName"];
            string Email = form["Email"];
            string Phone = form["Phone"];

            string City = form["City"];
            var TitleCity = _shopPhoneDbContext.Cities.Where(x=> x.Id == int.Parse(City)).First().Title;
            string District = form["District"];
            var TitleDistrict = _shopPhoneDbContext.Districts.Where(x => x.Id == int.Parse(District)).First().Title;
            string Ward = form["Ward"];
            var TitleWard = _shopPhoneDbContext.Wards.Where(x => x.Id == int.Parse(Ward)).First().Title;

            string Description = form["Description"];
            string AddressType = form["AddressType"];
            

            var item = new MyAddressData
            {
                FullName = FullName,
                CityName = TitleCity,
                DistrictName = TitleDistrict,
                WardName = TitleWard,
                Description = Description,
                AddressType = int.Parse(AddressType),
                IsDefault = 0,
                IdAccount = AccountId,
                Phone = Phone,
                Email = Email,

            };
            _userRepository.Create(item);

            

            return RedirectToAction("MyAddress", "Utilities");
        }

        [HttpPost]
        [Authorize]

        public IActionResult Update(IFormCollection form)
        {

            string IsDefault = form["IsDefault"];
            string Id = form["Id"];
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            int Isdefault_ParseInt = int.Parse(IsDefault);
            int Id_ParseInt = int.Parse(Id);


            _userRepository.Update(Isdefault_ParseInt, Id_ParseInt);



            TempData["AccountInt"] = AccountId;
            return RedirectToAction("Index");



        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var checkValue = await _userRepository.GetById(id);

            if(checkValue == null)
            {
                return Json(new { success = false });
            }

            _userRepository.Delete(checkValue.Id);


            return Json(new { success = true });
        }

    }
}
