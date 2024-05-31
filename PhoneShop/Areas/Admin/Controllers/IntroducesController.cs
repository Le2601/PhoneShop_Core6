using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.Introduce;
using PhoneShop.Models;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IntroducesController : Controller
    {


        private readonly ShopPhoneDbContext _context;
        private readonly IIntroduceRepository _introduce;
        public IntroducesController(ShopPhoneDbContext context, IIntroduceRepository introduceRepository)
        {
            _introduce = introduceRepository;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var item = await _introduce.GetIntroduce();
            return View(item);
        }

        public IActionResult Update(int id)
        {
            var item = _introduce.GetById(id);
            return View(item);
        }

        [HttpPost]
       public IActionResult Update(IntroduceData model)
        {
            _introduce.Update(model);

            return RedirectToAction("Index");
        }
    }
}
