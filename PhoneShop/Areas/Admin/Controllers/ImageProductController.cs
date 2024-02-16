using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using PhoneShop.DI.Category;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Helpper;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ImageProductController : Controller
    {
        private readonly ShopPhoneDbContext _context;



        public ImageProductController(ShopPhoneDbContext context)
        {
            _context = context;

        }
        public IActionResult Index(int? id)
        {
            var items = _context.ImageProducts.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        [HttpGet]
        public IActionResult GetListImage(int id)
        {
            var items = _context.ImageProducts.Where(x=> x.ProductId == id).ToList();



            if(items != null)
            {
                return Json(new { Data =  items, success = true, smg = "Hình ảnh của sản phẩm hiện có" });
            }

            return Json(new { Data = items, success = true, smg = "Không có hình ảnh" });

        }
    }
}
