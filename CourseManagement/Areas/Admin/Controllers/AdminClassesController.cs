using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminClassesController : Controller
    {
        private readonly CourseDatabaseContext _context;

        public AdminClassesController(CourseDatabaseContext context)
        {
            _context = context;
        }

        // GET: Admin/AdminClasses
        public async Task<IActionResult> Index()
        {
            var courseDatabaseContext = _context.Classes.Include(x => x.IdCalendarNavigation).Include(x => x.IdLecturerNavigation).Include(x => x.IdStudentNavigation);
            return View(await courseDatabaseContext.ToListAsync());
        }

        // GET: Admin/AdminClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(x => x.IdCalendarNavigation)
                .Include(x => x.IdLecturerNavigation)
                .Include(x => x.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdClass == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Admin/AdminClasses/Create
        public IActionResult Create()
        {
            ViewData["IdCalendar"] = new SelectList(_context.Calendars, "IdCalendar", "IdCalendar");
            ViewData["IdLecturer"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount");
            ViewData["IdStudent"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount");
            return View();
        }

        // POST: Admin/AdminClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdClass,IdLecturer,IdStudent,IdCalendar")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCalendar"] = new SelectList(_context.Calendars, "IdCalendar", "IdCalendar", @class.IdCalendar);
            ViewData["IdLecturer"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", @class.IdLecturer);
            ViewData["IdStudent"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", @class.IdStudent);
            return View(@class);
        }

        // GET: Admin/AdminClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            ViewData["IdCalendar"] = new SelectList(_context.Calendars, "IdCalendar", "IdCalendar", @class.IdCalendar);
            ViewData["IdLecturer"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", @class.IdLecturer);
            ViewData["IdStudent"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", @class.IdStudent);
            return View(@class);
        }

        // POST: Admin/AdminClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdClass,IdLecturer,IdStudent,IdCalendar")] Class @class)
        {
            if (id != @class.IdClass)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.IdClass))
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
            ViewData["IdCalendar"] = new SelectList(_context.Calendars, "IdCalendar", "IdCalendar", @class.IdCalendar);
            ViewData["IdLecturer"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", @class.IdLecturer);
            ViewData["IdStudent"] = new SelectList(_context.Accounts, "IdAccount", "IdAccount", @class.IdStudent);
            return View(@class);
        }

        // GET: Admin/AdminClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var @class = await _context.Classes
                .Include(x => x.IdCalendarNavigation)
                .Include(x => x.IdLecturerNavigation)
                .Include(x => x.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdClass == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Admin/AdminClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Classes == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Classes'  is null.");
            }
            var @class = await _context.Classes.FindAsync(id);
            if (@class != null)
            {
                _context.Classes.Remove(@class);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClassExists(int id)
        {
          return _context.Classes.Any(e => e.IdClass == id);
        }
    }
}
