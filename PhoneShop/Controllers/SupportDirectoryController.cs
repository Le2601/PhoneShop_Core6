using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.DI.DI_User.SupportDirectoryData;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class SupportDirectoryController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly ISupportDirectory_Repository _repository;


        public SupportDirectoryController(ShopPhoneDbContext context,ISupportDirectory_Repository supportDirectoryRepository)
        {
            _repository = supportDirectoryRepository;
            _context = context;
        }

        [Route("/support.html")]
        public async Task<IActionResult> Index()
        {

            var items = await _repository.GetAll();

            ViewBag.ListItemDetail_Support = await _context.Support_Contents.ToListAsync();

            return View(items);
        }
    }
}
