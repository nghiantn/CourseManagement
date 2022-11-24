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

        // GET: Calendars
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

        // GET: Calendars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Calendars == null)
            {
                return NotFound();
            }

            var calendar = await _context.Calendars
                .Include(c => c.IdCourseNavigation)
                .Include(c => c.IdTeacherNavigation)
                .FirstOrDefaultAsync(m => m.IdCalendar == id);
            if (calendar == null)
            {
                return NotFound();
            }

            return View(calendar);
        }

        // GET: Calendars/Create
        public IActionResult Create()
        {
            ViewData["IdCourse"] = new SelectList(_context.Courses, "IdCourse", "IdCourse");
            ViewData["IdTeacher"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount");
            return View();
        }

        // POST: Calendars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCalendar,Name,StartTime,EndTime,Length,IdCourse,IdTeacher,Slotnow,Slotmax,Active")] Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(calendar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCourse"] = new SelectList(_context.Courses, "IdCourse", "IdCourse", calendar.IdCourse);
            ViewData["IdTeacher"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", calendar.IdTeacher);
            return View(calendar);
        }

        // GET: Calendars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Calendars == null)
            {
                return NotFound();
            }

            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar == null)
            {
                return NotFound();
            }
            ViewData["IdCourse"] = new SelectList(_context.Courses, "IdCourse", "IdCourse", calendar.IdCourse);
            ViewData["IdTeacher"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", calendar.IdTeacher);
            return View(calendar);
        }

        // POST: Calendars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCalendar,Name,StartTime,EndTime,Length,IdCourse,IdTeacher,Slotnow,Slotmax,Active")] Calendar calendar)
        {
            if (id != calendar.IdCalendar)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(calendar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CalendarExists(calendar.IdCalendar))
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
            ViewData["IdCourse"] = new SelectList(_context.Courses, "IdCourse", "IdCourse", calendar.IdCourse);
            ViewData["IdTeacher"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", calendar.IdTeacher);
            return View(calendar);
        }

        // GET: Calendars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Calendars == null)
            {
                return NotFound();
            }

            var calendar = await _context.Calendars
                .Include(c => c.IdCourseNavigation)
                .Include(c => c.IdTeacherNavigation)
                .FirstOrDefaultAsync(m => m.IdCalendar == id);
            if (calendar == null)
            {
                return NotFound();
            }

            return View(calendar);
        }

        // POST: Calendars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Calendars == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Calendars'  is null.");
            }
            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar != null)
            {
                _context.Calendars.Remove(calendar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarExists(int id)
        {
          return _context.Calendars.Any(e => e.IdCalendar == id);
        }
    }
}
