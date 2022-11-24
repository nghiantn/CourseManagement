using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;

namespace CourseManagement.Controllers
{
    public class ContactsController : Controller
    {
        private readonly CourseDatabaseContext _context;

        public ContactsController(CourseDatabaseContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var courseDatabaseContext = _context.Contacts.Include(c => c.IdCalendarNavigation).Include(c => c.IdStatusNavigation);
            return View(await courseDatabaseContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["IdCalendar"] = new SelectList(_context.Calendars, "IdCalendar", "IdCalendar");
            ViewData["IdStatus"] = new SelectList(_context.Statuses, "IdStatus", "Description");
            return View();
        }

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

    }
}
