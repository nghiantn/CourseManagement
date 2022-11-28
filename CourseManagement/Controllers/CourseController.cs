using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace CourseManagement.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public CourseController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index(int page = 1, int IdCategory = 0)
        {
            var pageNumber = page;
            var pageSize = 12;

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

        public async Task<IActionResult> Details(int? id)
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
    }
}
