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
using PhoneShop.DI.ImageProduct;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ImageProductController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        private readonly IImageProductRepository _imageProductRepository;

        public ImageProductController(ShopPhoneDbContext context, IImageProductRepository imageProductRepository)
        {
            _imageProductRepository = imageProductRepository;
            _context = context;

        }
        public IActionResult Index(int? id)
        {
            var items = _imageProductRepository.GetListByIdProduct(id);
            return View(items);
        }

        [HttpGet]
        public IActionResult GetListImage(int id)
        {
            var items = _imageProductRepository.GetListByIdProduct(id);



            if(items != null)
            {
                return Json(new { Data =  items, success = true, smg = "Hình ảnh của sản phẩm hiện có" });
            }

            return Json(new { Data = items, success = true, smg = "Không có hình ảnh" });

        }

        [Route("/delete_Image/{Id}")]
        public async Task<IActionResult> DelImage( int Id)
        {
            var item = _imageProductRepository.GetById(Id);

            if(item == null)
            {

                return RedirectToAction("NotFoundApp","Home");
            }

            string pathimg = "/Product/" + item.DataName;
            //xoa hinh anh trong folder
            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product/" + item.DataName);
            if (System.IO.File.Exists(pathFile))
            {
                // Xóa hình ảnh
                System.IO.File.Delete(pathFile);

            }

            _imageProductRepository.DeleteImage(item.Id);
           


            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());
        }

       




    }
}
