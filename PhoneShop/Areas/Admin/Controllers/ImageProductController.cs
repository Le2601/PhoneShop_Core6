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

       
       




    }
}
