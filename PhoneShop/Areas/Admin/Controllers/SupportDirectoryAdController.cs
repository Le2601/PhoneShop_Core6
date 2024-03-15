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
    public class SupportDirectoryAdController : Controller
    {
        private readonly ISupportDirectoryRepository _repository;

        public SupportDirectoryAdController(ISupportDirectoryRepository repository)
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
            if (ModelState.IsValid)
            {
                return View(model);

            }
            model.Alias = PhoneShop.Helpper.Utilities.SEOUrl(model.Title);
             _repository.Create(model);

            return RedirectToAction("Index");

        }

        public IActionResult Update(int id)
        {
            if(_repository.CheckId(id)== 0) {

                return RedirectToAction("NotFoundApp", "Home");
            }
            
            var item = _repository.GetById(id);

            return View(item);
        }
        [HttpPost]

        public IActionResult Update(SupportDirectoryData model)
        {
            _repository.Update(model);
            

            return RedirectToAction("Index", "SupportDirectoryAd");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (_repository.CheckId(id) == 0)
            {

                return RedirectToAction("NotFoundApp", "Home");
            }

            _repository.Delete(id);


            return Json(new { success = true });
        }


    }
}
