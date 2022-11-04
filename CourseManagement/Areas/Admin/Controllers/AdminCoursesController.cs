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
            ViewBag.CurrentIdCategory = IdCategory;
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name");
            return View(models);
        }

        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name");
            return View();
        }

        // POST: Admin/AdminCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCourse,Name,Description,IdCategory,Image,Price,Active")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name", course.IdCategory);
            return View(course);
        }

        // GET: Admin/AdminCourses/Edit/5
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
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name", course.IdCategory);
            return View(course);
        }

        // POST: Admin/AdminCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCourse,Name,Description,IdCategory,Image,Price,Active")] Course course)
        {
            if (id != course.IdCourse)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "IdCategory", "Name", course.IdCategory);
            return View(course);
        }

        // GET: Admin/AdminCourses/Delete/5
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

            return View(course);
        }

        // POST: Admin/AdminCourses/Delete/5
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
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return _context.Courses.Any(e => e.IdCourse == id);
        }
    }
}
