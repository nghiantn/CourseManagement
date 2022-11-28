using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace CourseManagement.Controllers
{
    public class ContactsController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public ContactsController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Create(int IdCalendar = 0)
        {
            ViewData["IdCalendar"] = new SelectList(_context.Calendars.Where(x => x.IdCalendar == IdCalendar), "IdCalendar", "Name");
            ViewData["IdStatus"] = new SelectList(_context.Statuses.Where(x => x.IdStatus == 1), "IdStatus", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdContact,Phone,Email,Fullname,IdCalendar,IdStatus")] Contact contact, int IdCalendar = 0)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                _notyf.Success("Đã gửi yêu cầu đăng ký");
                return RedirectToAction("Index", "Course");
            }
            ViewData["IdCalendar"] = new SelectList(_context.Calendars.Where(x => x.IdCalendar == IdCalendar), "IdCalendar", "Name", contact.IdCalendar);
            ViewData["IdStatus"] = new SelectList(_context.Statuses.Where(x => x.IdStatus == 1), "IdStatus", "Name", contact.IdStatus);
            return View(contact);
        }
        
    }
}
