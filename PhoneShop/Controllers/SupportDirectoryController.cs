using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.DI.DI_User.SupportContentData;
using PhoneShop.DI.DI_User.SupportDirectoryData;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class SupportDirectoryController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly ISupportDirectory_Repository _repository;
        private readonly ISupportContent_Repository _contentRepository;


        public SupportDirectoryController(ShopPhoneDbContext context,ISupportDirectory_Repository supportDirectoryRepository, ISupportContent_Repository supportContent_Repository)
        {
            _contentRepository = supportContent_Repository;
            _repository = supportDirectoryRepository;
            _context = context;
        }

        [Route("/support.html")]
        public async Task<IActionResult> Index()
        {

            var items = await _repository.GetAll();

            ViewBag.ListItemDetail_Support = await _contentRepository.GetAll();

            return View(items);
        }

        [Route("/Support_detail/{Alias}-{Id}")]
        public async Task<IActionResult> Detail_SupportContent(int Id)
        {
            var item =await _contentRepository.GetById(Id);

            return View(item);
        }



    }
}
