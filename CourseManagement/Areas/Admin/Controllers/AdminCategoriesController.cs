using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Rewrite;
using PagedList.Core;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class AdminCategoriesController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminCategoriesController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }


        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Category> lsCategories = new List<Category>();

            lsCategories = _context.Categories
            .AsNoTracking()
                .OrderBy(x => x.IdCategory).ToList();

            PagedList<Category> models = new PagedList<Category>(lsCategories.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsCategories.Count / (Double)pageSize);

            return View(models);
        }


        public IActionResult Create()
        {
            _notyf.Information("Đang thêm mới");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategory,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                _notyf.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

       
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            _notyf.Warning("Đang chỉnh sửa");
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategory,Name,Description")] Category category)
        {
            if (id != category.IdCategory)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.IdCategory))
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
            _notyf.Success("Chỉnh sửa thành công");
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.IdCategory == id);
            if (category == null)
            {
                return NotFound();
            }
            _notyf.Error("Xác nhận xóa");
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            _notyf.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return _context.Categories.Any(e => e.IdCategory == id);
        }
    }
}
