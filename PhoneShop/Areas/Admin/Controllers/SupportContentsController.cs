using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.SupportContent;
using PhoneShop.DI.SupportDirectory;
using PhoneShop.Models;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupportContentsController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        private readonly ISupportContentRepository _supportContentRepository;

        private readonly ISupportDirectoryRepository _supportDirectoryRepository;

        public SupportContentsController(ShopPhoneDbContext context, ISupportContentRepository supportContentRepository, ISupportDirectoryRepository supportDirectoryRepository)
        {
            _supportDirectoryRepository = supportDirectoryRepository;
            _supportContentRepository = supportContentRepository;
            _context = context;
        }

        // GET: Admin/SupportContents
        public async Task<IActionResult> Index()
        {
            var items = await _supportContentRepository.GetAll();

            ViewBag.ListDirectory = _supportDirectoryRepository.GetAll();

            return View(items);
        }

        // GET: Admin/SupportContents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportContent = await _context.Support_Contents
                .Include(s => s.SupportDirectory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supportContent == null)
            {
                return NotFound();
            }

            return View(supportContent);
        }

        // GET: Admin/SupportContents/Create
        public IActionResult Create()
        {
            ViewData["IdSpDirectory"] = new SelectList(_supportDirectoryRepository.GetAll(), "Id", "Title");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupportContentData supportContent)
        {
            supportContent.Alias = PhoneShop.Helpper.Utilities.SEOUrl(supportContent.Title);
            if (!ModelState.IsValid)
            { 
                _supportContentRepository.Create(supportContent);
                return RedirectToAction(nameof(Index));
               
            }
            ViewData["IdSpDirectory"] = new SelectList(_supportDirectoryRepository.GetAll(), "Id", "Title", supportContent.IdSpDirectory);
            return View(supportContent);
        }

        // GET: Admin/SupportContents/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if(_supportContentRepository.CheckId(id) == 0)
            {
                return RedirectToAction("NotFoundApp", "Home");
            }

            var supportContent = await _supportContentRepository.GetById(id);
           
            ViewData["IdSpDirectory"] = new SelectList(_supportDirectoryRepository.GetAll(), "Id", "Id", supportContent.IdSpDirectory);
            return View(supportContent);
        }

        // POST: Admin/SupportContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SupportContentData model)
        {
           
            _supportContentRepository.Update(model);
            return RedirectToAction(nameof(Index));
            
        }

        [HttpPost] 
        public IActionResult Delete(int id)
        {
            _supportContentRepository.Delete(id);
            return Json(new { success = true });
        }

        private bool SupportContentExists(int id)
        {
            return _context.Support_Contents.Any(e => e.Id == id);
        }
    }
}
