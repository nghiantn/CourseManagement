using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using PagedList.Core;

namespace CourseManagement.Controllers
{
    public class CalendarsController : Controller
    {
        private readonly CourseDatabaseContext _context;

        public CalendarsController(CourseDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index(int IdCourse = 0, int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Calendar> lsCalendars = new List<Calendar>();

            if (IdCourse != 0)
            {
                lsCalendars = _context.Calendars
                .AsNoTracking()
                .Where(x => x.IdCourse == IdCourse)
                .Include(x => x.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .Where(x => x.Active)
                .OrderByDescending(x => x.IdCalendar).ToList();
            }
            else
            {
                lsCalendars = _context.Calendars
                .AsNoTracking()
                .Include(x => x.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .Where(x => x.Active)
                .OrderByDescending(x => x.IdCalendar).ToList();
            }

            PagedList<Calendar> models = new PagedList<Calendar>(lsCalendars.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsCalendars.Count / (Double)pageSize);
            ViewBag.CurrentIdCourse = IdCourse;
            ViewData["IdCourse"] = new SelectList(_context.Courses, "IdCourse", "Name");
            ViewData["IdTeacher"] = new SelectList(_context.Accounts.Where(x => x.IdRoleNavigation.Name == "Teacher"), "IdAccount", "Fullname");
            return View(models);
        }

    }
}
