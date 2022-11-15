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
using PagedList.Core;
using System.IO;
using CourseManagement.Helpper;
using Microsoft.AspNetCore.Hosting;



namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class AdminCoursesController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminCoursesController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        public IActionResult Index(int page = 1, int IdCategory = 0)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Course> lsCourses = new List<Course>();

            if (IdCategory != 0)
            {
                lsCourses = _context.Courses
                .AsNoTracking()
                .Where(x => x.IdCategory == IdCategory)
                .Include(x => x.IdCategoryNavigation)
                .OrderBy(x => x.IdCourse).ToList();
            }
            else
            {
                lsCourses = _context.Courses
                .AsNoTracking()
                .Include(x => x.IdCategoryNavigation)
                .OrderBy(x => x.IdCourse).ToList();
            }

            PagedList<Course> models = new PagedList<Course>(lsCourses.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsCourses.Count / (Double)pageSize);
            ViewBag.CurrentIdCategory = IdCategory;
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name");
            return View(models);
        }

        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name");
            _notyf.Information("Đang thêm mới");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCourse,Name,Description,IdCategory,Image,Price,Active")] Course course, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                 if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(course.Name) + extension;
                        course.Image = await Utilities.UploadFile(fThumb, @"Courses", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(course.Image)) course.Image = "default.jpg";
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name", course.IdCategory);
            _notyf.Success("Tạo mới thành công");
            return View(course);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            _notyf.Warning("Đang chỉnh sửa");
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name", course.IdCategory);
            return View(course);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCourse,Name,Description,IdCategory,Image,Price,Active")] Course course, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != course.IdCourse)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(course.Name) + extension;
                        course.Image = await Utilities.UploadFile(fThumb, @"Courses", image.ToLower());
                    }

                    if (string.IsNullOrEmpty(course.Image)) course.Image = "default.jpg";

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.IdCourse))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Success("Chỉnh sửa thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name", course.IdCategory);
            return View(course);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.IdCategoryNavigation)
                .FirstOrDefaultAsync(m => m.IdCourse == id);
            if (course == null)
            {
                return NotFound();
            }
            _notyf.Error("Xác nhận xóa");
            return View(course);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Courses'  is null.");
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }
            
            await _context.SaveChangesAsync();
            _notyf.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return _context.Courses.Any(e => e.IdCourse == id);
        }
    }
}
