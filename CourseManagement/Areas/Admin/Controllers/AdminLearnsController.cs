using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Globalization;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class AdminLearnsController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminLearnsController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }


        public async Task<IActionResult> Index(int IdCalendar = 0, int page = 1)
        {
            var calendar = await _context.Calendars
                  .Include(c => c.IdCourseNavigation)
                  .Include(x => x.IdTeacherNavigation)
                  .SingleOrDefaultAsync(m => m.IdCalendar == IdCalendar);

            if (calendar == null)
            {
                return RedirectToAction("Index", "AdminCalendars");
            }

            if (calendar.IdCourseNavigation.Active == false)
            {
                calendar.Active = false;
                _context.Calendars.Update(calendar);

                await _context.SaveChangesAsync();

                _notyf.Error("Khóa học không còn hoạt động");
                _notyf.Warning("Đã đóng lịch học");

                return RedirectToAction("Index","AdminCalendars");
            }
            else
            {
                var pageNumber = page;
                var pageSize = 10;

                List<Learn> lsLearns = new List<Learn>();

                lsLearns = _context.Learns
                    .AsNoTracking()
                    .Where(x => x.IdCalendar == IdCalendar)
                    .Include(x => x.IdCalendarNavigation)
                    .Include(x => x.IdStudentNavigation)
                    .OrderBy(x => x.IdStudent).ToList();

                PagedList<Learn> models = new PagedList<Learn>(lsLearns.AsQueryable(), pageNumber, pageSize);

                ViewBag.CurrentPage = pageNumber;
                ViewBag.PageLength = (int)Math.Ceiling((Double)lsLearns.Count / (Double)pageSize);
                ViewBag.CurrentIdCalendar = IdCalendar;
                return View(models);
            }
        }

        public async Task<IActionResult> Create(int IdCalendar = 0)
        {

            var calendar = await _context.Calendars
                .Include(c => c.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .FirstOrDefaultAsync(m => m.IdCalendar == IdCalendar);

            if (calendar.Slotnow == calendar.Slotmax)
            {
                _notyf.Warning("Lịch học đã hết chỗ");
                return RedirectToAction(nameof(Index), new { IdCalendar });
            }


            ViewData["IdCalendar"] = new SelectList(_context.Calendars.Where(x => x.IdCalendar == IdCalendar), "IdCalendar", "Name");
            ViewData["IdStudent"] = new SelectList(_context.Accounts.Where(x => (x.IdRoleNavigation.Name == "Student" && x.Active)), "IdAccount", "Fullname");
            _notyf.Information("Đang thêm học viên");
            ViewBag.CurrentIdCalendar = IdCalendar;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdStudent,IdCalendar,IdLearn")] Learn learn, int IdCalendar)
        {
            if (ModelState.IsValid)
            {
                var calendar = await _context.Calendars
                .Include(c => c.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .FirstOrDefaultAsync(m => m.IdCalendar == learn.IdCalendar);

                var _learn = await _context.Learns
                .Include(c => c.IdCalendarNavigation)
                .Include(x => x.IdStudentNavigation)
                .Where(m => m.IdCalendar == learn.IdCalendar)
                .FirstOrDefaultAsync(m => m.IdStudent == learn.IdStudent);

                if (calendar == null || _learn != null)
                {
                    _notyf.Warning("Học viên đã được thêm");
                    return RedirectToAction(nameof(Index), new { IdCalendar });
                }

                calendar.Slotnow++;
                _context.Update(calendar);

                learn.IdCalendar = IdCalendar;
                _context.Add(learn);

                await _context.SaveChangesAsync();
                _notyf.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index), new { IdCalendar });
            }

            _notyf.Error("Tạo mới không thành công");
            return RedirectToAction(nameof(Index), new { IdCalendar });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Learns == null)
            {
                return NotFound();
            }

            var learn = await _context.Learns
                .Include(c => c.IdCalendarNavigation)
                .Include(x => x.IdStudentNavigation)
                .FirstOrDefaultAsync(m => m.IdLearn == id);

            int IdCalendar = learn.IdCalendar;

            if (learn == null)
            {
                _notyf.Error("Đã xảy ra lỗi khi xóa");
                return RedirectToAction(nameof(Index), new { IdCalendar });
            }

            _notyf.Error("Xác nhận xóa học viên");
            return View(learn);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Learns == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Learns'  is null.");
            }
            var learn = await _context.Learns.FindAsync(id);

            int IdCalendar = learn.IdCalendar;

            if (learn != null)
            {
                var calendar = await _context.Calendars
                .Include(c => c.IdCourseNavigation)
                .Include(x => x.IdTeacherNavigation)
                .FirstOrDefaultAsync(m => m.IdCalendar == learn.IdCalendar);

                if (calendar == null)
                {
                    _notyf.Error("Đã xảy ra lỗi khi xóa");
                    return RedirectToAction(nameof(Index), new { IdCalendar });
                }

                calendar.Slotnow--;
                _context.Update(calendar);
                _context.Learns.Remove(learn);
            }

            await _context.SaveChangesAsync();
            _notyf.Success("Xóa thành công");
            return RedirectToAction(nameof(Index), new { IdCalendar });
        }

        private bool LearnExists(int id)
        {
            return _context.Learns.Any(e => e.IdLearn == id);
        }
    }
}
