using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupportContentsController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public SupportContentsController(ShopPhoneDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SupportContents
        public async Task<IActionResult> Index()
        {
            var shopPhoneDbContext = _context.Support_Contents.Include(s => s.SupportDirectory);
            return View(await shopPhoneDbContext.ToListAsync());
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
            ViewData["IdSpDirectory"] = new SelectList(_context.Support_Directories, "Id", "Title");
            return View();
        }

        // POST: Admin/SupportContents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Alias,Content,IdSpDirectory")] SupportContent supportContent)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(supportContent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSpDirectory"] = new SelectList(_context.Support_Directories, "Id", "Title", supportContent.IdSpDirectory);
            return View(supportContent);
        }

        // GET: Admin/SupportContents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supportContent = await _context.Support_Contents.FindAsync(id);
            if (supportContent == null)
            {
                return NotFound();
            }
            ViewData["IdSpDirectory"] = new SelectList(_context.Support_Directories, "Id", "Id", supportContent.IdSpDirectory);
            return View(supportContent);
        }

        // POST: Admin/SupportContents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Alias,Content,IdSpDirectory")] SupportContent supportContent)
        {
            if (id != supportContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportContentExists(supportContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdSpDirectory"] = new SelectList(_context.Support_Directories, "Id", "Id", supportContent.IdSpDirectory);
            return View(supportContent);
        }

        // GET: Admin/SupportContents/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Admin/SupportContents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supportContent = await _context.Support_Contents.FindAsync(id);
            _context.Support_Contents.Remove(supportContent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportContentExists(int id)
        {
            return _context.Support_Contents.Any(e => e.Id == id);
        }
    }
}
