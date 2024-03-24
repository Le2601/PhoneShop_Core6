using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using OfficeOpenXml;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.Voucher;
using PhoneShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherAdController : Controller
    {
        private readonly ShopPhoneDbContext db;
        private readonly IVoucherRepository _voucherRepository;


        public VoucherAdController(ShopPhoneDbContext context, IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
            db = context;

        }
        public async Task<IActionResult> Index()
        {
            var items =await _voucherRepository.GetAll();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VoucherData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            }
            _voucherRepository.Create(model);
            return RedirectToAction("Index", "VoucherAd");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item =await _voucherRepository.GetById(id);
            if (_voucherRepository.CheckId(id) == 0)
            {
                return RedirectToAction("NotFoundApp", "Home");
            }


            _voucherRepository.Delete(id);

            return Json(new { success = true});
        }

        public async Task<IActionResult> Update(int id)
        {
            if(_voucherRepository.CheckId(id) == 0)
            {
                return RedirectToAction("NotFoundApp", "Home");
            }

            var item =await _voucherRepository.GetById(id);       

            return View(item);
        }

        [HttpPost]
        public IActionResult Update(VoucherData  model) 
        {

           
            _voucherRepository.Update(model);




            return RedirectToAction("Index", "VoucherAd");
        }
       





    }
}
