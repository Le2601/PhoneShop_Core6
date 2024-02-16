using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PhoneShop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VoucherController : Controller
    {
        private readonly ShopPhoneDbContext db;



        public VoucherController(ShopPhoneDbContext context)
        {
            db = context;

        }
        public IActionResult Index()
        {
            var items = db.Vouchers.ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Voucher model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            
            }
            db.Vouchers.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        


    }
}
