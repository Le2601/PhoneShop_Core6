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
using PhoneShop.DI.Role;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class RoleController : Controller
    {
        private readonly IRoleRepository _RoleRepository;
        private readonly ShopPhoneDbContext _dbContext;
        public RoleController(IRoleRepository roleRepository, ShopPhoneDbContext dbContext)
        {
            _RoleRepository = roleRepository;

            _dbContext = dbContext;



        }



      

        public async Task<ActionResult> Index()
        {
            var items = await _RoleRepository.GetAll();

            return View(items);
        }
        public IActionResult Create()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model)
        {
            if (!ModelState.IsValid)
            {
                //kiem tra neu trung ten
                var CheckTitle = _dbContext.Roles.Where(x => x.RoleName == model.RoleName).ToList();
                if (CheckTitle.Any())
                {
                    return StatusCode(404, "Tên đã tồn tại");
                }


                var CreateI = await _RoleRepository.Create(model);

                return RedirectToAction("Index");

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _dbContext.Roles.Where(x => x.Id == id).FirstOrDefault();

            if (item != null)
            {
                var items = await _RoleRepository.Delete(id);

                return Json(new { success = true, msg = "Xóa thành công" });

            }
            return Json(new { success = false, msg = "Xóa thất bại" });




        }
    }
}
