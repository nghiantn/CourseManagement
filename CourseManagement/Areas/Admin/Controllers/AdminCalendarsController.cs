using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Rewrite;
using PagedList.Core;
using System.Globalization;
using Calendar = CourseManagement.Models.Calendar;
using Microsoft.AspNetCore.Authorization;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminCalendarsController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminCalendarsController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
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

        public IActionResult IndexFalse(int IdCourse = 0, int page = 1)
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
                .Where(x => !x.Active)
                .OrderByDescending(x => x.IdCalendar).ToList();
            }
            else
            {
                lsCalendars = _context.Calendars
                .AsNoTracking()
                .Include(x => x.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .Where(x => !x.Active)
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

        public IActionResult Create()
        {
            ViewData["IdCourse"] = new SelectList(_context.Courses.Where(x => x.Active), "IdCourse", "Name");
            ViewData["IdTeacher"] = new SelectList(_context.Accounts.Where(x => x.IdRoleNavigation.Name == "Teacher"), "IdAccount", "Fullname");
            _notyf.Information("Đang thêm mới");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCalendar,Name,StartTime,EndTime,Length,IdCourse,IdTeacher,Slotmax,Active")] Calendar calendar)
        {
            if (ModelState.IsValid)
            {
                calendar.Length = calendar.EndTime.Subtract(calendar.StartTime).Days;
                calendar.Slotnow = 0;
                _context.Add(calendar);
                await _context.SaveChangesAsync();
                _notyf.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCourse"] = new SelectList(_context.Courses.Where(x => x.Active), "IdCourse", "Name", calendar.IdCourse);
            ViewData["IdTeacher"] = new SelectList(_context.Accounts.Where(x => x.IdRoleNavigation.Name == "Teacher"), "IdAccount", "Fullname", calendar.IdTeacher);
            return View(calendar);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Calendars == null)
            {
                return NotFound();
            }

            var calendar = await _context.Calendars
                   .Include(c => c.IdCourseNavigation)
                   .Include(x => x.IdTeacherNavigation)
                   .SingleOrDefaultAsync(m => m.IdCalendar == id.Value);

            if (calendar == null)
            {
                _notyf.Warning("Đã xãy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            if (calendar.IdCourseNavigation.Active == false)
            {
                calendar.Active = false;
                _context.Calendars.Update(calendar);

                await _context.SaveChangesAsync();

                _notyf.Error("Khóa học không còn hoạt động");
                _notyf.Warning("Đã đóng lịch học");

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCourse"] = new SelectList(_context.Courses.Where(x => x.Active), "IdCourse", "Name", calendar.IdCourse);
            ViewData["IdTeacher"] = new SelectList(_context.Accounts.Where(x => x.IdRoleNavigation.Name == "Teacher"), "IdAccount", "Fullname", calendar.IdTeacher);
            _notyf.Warning("Đang chỉnh sửa");
            return View(calendar);
        }


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
                    calendar.Length = calendar.EndTime.Subtract(calendar.StartTime).Days;

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
                _notyf.Success("Chỉnh sửa thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCourse"] = new SelectList(_context.Courses.Where(x => x.Active), "IdCourse", "Name", calendar.IdCourse);
            ViewData["IdTeacher"] = new SelectList(_context.Accounts.Where(x => x.IdRoleNavigation.Name == "Teacher"), "IdAccount", "Fullname", calendar.IdTeacher);
            return View(calendar);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Calendars == null)
            {
                return NotFound();
            }

            var calendar = await _context.Calendars
                .Include(c => c.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .FirstOrDefaultAsync(m => m.IdCalendar == id);
            if (calendar == null)
            {
                return NotFound();
            }
            _notyf.Error("Xác nhận xóa");
            return View(calendar);
        }

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
            _notyf.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Open(int id)
        {
            var calendar = await _context.Calendars
                   .Include(c => c.IdCourseNavigation)
                   .Include(x => x.IdTeacherNavigation)
                   .SingleOrDefaultAsync(m => m.IdCalendar == id);

            if (calendar == null)
            {
                _notyf.Error("Đã xãy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            calendar.Active = true;
            _context.Calendars.Update(calendar);

            await _context.SaveChangesAsync();
            _notyf.Success("Mở lịch học thành công");
            return RedirectToAction(nameof(IndexFalse));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenWithCourse(int id)
        {
            var calendar = await _context.Calendars
                   .Include(c => c.IdCourseNavigation)
                   .Include(x => x.IdTeacherNavigation)
                   .SingleOrDefaultAsync(m => m.IdCalendar == id);

            var course = await _context.Courses
                   .Include(c => c.IdCategoryNavigation)
                   .SingleOrDefaultAsync(m => m.IdCourse == calendar.IdCourse);

            if (calendar == null || course == null)
            {
                _notyf.Error("Đã xãy ra lỗi");
                return RedirectToAction(nameof(Index));
            }

            course.Active = true;
            calendar.Active = true;

            _context.Courses.Update(course); 
            _context.Calendars.Update(calendar);
            
            await _context.SaveChangesAsync();
            _notyf.Success("Mở khóa học thành công");
            _notyf.Success("Mở lịch học thành công");
            return RedirectToAction(nameof(IndexFalse));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Close(int id)
        {
            if (_context.Calendars == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Calendars'  is null.");
            }
            var calendar = await _context.Calendars.FindAsync(id);
            if (calendar != null)
            {
                calendar.Active = false;
                _context.Calendars.Update(calendar);
            }

            await _context.SaveChangesAsync();
            _notyf.Success("Đóng lịch học thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool CalendarExists(int id)
        {
            return _context.Calendars.Any(e => e.IdCalendar == id);
        }
    }
}
