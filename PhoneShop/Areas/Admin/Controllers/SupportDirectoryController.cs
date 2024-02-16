using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using PhoneShop.DI.SupportDirectory;
using PhoneShop.Areas.Admin.Data;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SupportDirectoryController : Controller
    {
        private readonly ISupportDirectoryRepository _repository;

        public SupportDirectoryController(ISupportDirectoryRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            var iList = _repository.GetAll();

            return View(iList);
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(SupportDirectoryData model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
                
            }
            model.Alias = PhoneShop.Helpper.Utilities.SEOUrl(model.Title);
            var Create = _repository.Create(model);

            return RedirectToAction("Index");

        }
    }
}
