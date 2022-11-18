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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Rewrite;
using PagedList.Core;
using CourseManagement.Extension;
using CourseManagement.Helpper;
using System.Xaml.Permissions;
using System.Security.Cryptography.Xml;
using System.Globalization;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class AdminContactsController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminContactsController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult Index(int IdStatus = 0, int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Contact> lsContacts = new List<Contact>();

            if (IdStatus != 0)
            {
                lsContacts = _context.Contacts
                .AsNoTracking()
                .Where(x => x.IdStatus == IdStatus)
                .Include(x => x.IdCalendarNavigation)
                .Include(x => x.IdStatusNavigation)
                .OrderBy(x => x.IdStatus).ToList();
            }
            else
            {
                lsContacts = _context.Contacts
                .AsNoTracking()
                .Include(x => x.IdCalendarNavigation)
                .Include(x => x.IdStatusNavigation)
                .OrderBy(x => x.IdStatus).ToList();
            }

            PagedList<Contact> models = new PagedList<Contact>(lsContacts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsContacts.Count / (Double)pageSize);

            ViewBag.CurrentIdStatus = IdStatus;
            ViewBag.MaxStatus = _context.Statuses.Count();

            ViewData["IdCalendar"] = new SelectList(_context.Calendars, "IdCalendar", "Name");
            ViewData["IdStatus"] = new SelectList(_context.Statuses, "IdStatus", "Name");

            return View(models);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contacts == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }

            _notyf.Warning("Đang thay đổi lịch học");
            ViewData["IdCalendar"] = new SelectList(_context.Calendars.Where(x => x.Active), "IdCalendar", "Name", contact.IdCalendar);
            ViewData["IdStatus"] = new SelectList(_context.Statuses.Where(x => x.IdStatus == contact.IdStatus), "IdStatus", "Name", contact.IdStatus);
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdContact,Phone,Email,Fullname,IdCalendar,IdStatus")] Contact contact)
        {
            if (id != contact.IdContact)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.IdContact))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Success("Thay đổi lịch học thành công");
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCalendar"] = new SelectList(_context.Calendars.Where(x => x.Active), "IdCalendar", "Name", contact.IdCalendar);
            ViewData["IdStatus"] = new SelectList(_context.Statuses.Where(x => x.IdStatus == contact.IdStatus), "IdStatus", "Name", contact.IdStatus);
            return View(contact);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Contacts == null)
        //    {
        //        return NotFound();
        //    }

        //    var contact = await _context.Contacts
        //        .Include(c => c.IdCalendarNavigation)
        //        .Include(c => c.IdStatusNavigation)
        //        .SingleOrDefaultAsync(m => m.IdContact == id);

        //    if (contact == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(contact);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Contacts == null)
        //    {
        //        return Problem("Entity set 'CourseDatabaseContext.Contacts'  is null.");
        //    }
        //    var contact = await _context.Contacts.FindAsync(id);
        //    if (contact != null)
        //    {
        //        _context.Contacts.Remove(contact);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Contacts'  is null.");
            }

            var contact = await _context.Contacts.FindAsync(id);

            int maxstatus = _context.Statuses.Count();

            if (contact == null || contact.IdStatus + 1 >= maxstatus)
            {
                _notyf.Warning("Không thực hiện được");
                return RedirectToAction(nameof(Index));
            }

            else
            {
                var calendar = await _context.Calendars
                   .Include(c => c.IdCourseNavigation)
                   .Include(x => x.IdTeacherNavigation)
                   .SingleOrDefaultAsync(m => m.IdCalendar == contact.IdCalendar);

                if (calendar.Slotnow == calendar.Slotmax)
                {
                    _notyf.Error("Lịch học đầy");
                    _notyf.Information("Vui lòng liên hệ lại học viên");
                }
                else
                {
                    if (calendar.Active == false)
                    {
                        _notyf.Error("Lịch học đã đóng");
                        _notyf.Information("Vui lòng liên hệ lại học viên");
                    }
                    else
                    {
                        if (contact.IdStatus == 4)
                        {
                            if (_context.Accounts.SingleOrDefault(x => (x.Phone.Trim() == contact.Phone.Trim()) || x.Fullname.Trim() == contact.Fullname.Trim()) != null)
                            {
                                _notyf.Warning("Thông tin đã có");
                                return RedirectToAction(nameof(Index));
                            }

                            Account account = new Account
                            {
                                Phone = contact.Phone.Trim(),
                                Fullname = contact.Fullname.Trim(),
                                IdRole = 3,
                                Password = "123456".Trim().ToMD5(),
                                Active = true,
                                Username = Utilities.toUsername(contact.Fullname.Trim(), contact.Phone.Trim()),
                            };

                            _context.Accounts.Add(account);

                            _notyf.Success("Đã tạo tài khoản tự động");
                        }

                        contact.IdStatus++;
                        _context.Contacts.Update(contact);
                        await _context.SaveChangesAsync();

                        if (contact.IdStatus == 5)
                        {
                            await AddToLearn(contact);
                        }

                        _notyf.Success("Đã chuyển trạng thái");
                    }
                }
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> AddToLearn(Contact contact)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Username.Trim() == Utilities.toUsername(contact.Fullname.Trim(), contact.Phone.Trim()));
            var calendar = await _context.Calendars.SingleOrDefaultAsync(x => x.IdCalendar == contact.IdCalendar);

            if (account == null || calendar == null)
            {
                _notyf.Error("Không thể thêm lịch học");
            }
            else
            {
                Learn learn = new Learn
                {
                    IdCalendar = contact.IdCalendar,
                    IdStudent = account.IdAccount,
                };

                calendar.Slotnow++;
                _context.Update(calendar);
                _context.Add(learn);
                _notyf.Success("Đã thêm lịch học tự động");

                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelStatus(int id)
        {
            if (_context.Contacts == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Contacts'  is null.");
            }

            var contact = await _context.Contacts.FindAsync(id);

            int maxstatus = _context.Statuses.Count();

            if (contact == null || contact.IdStatus == maxstatus)
            {
                _notyf.Error("Không thực hiện được");
            }
            else
            {
                contact.IdStatus = maxstatus;

                _context.Contacts.Update(contact);

                _notyf.Success("Đã hủy yêu cầu");

                try
                {
                    if (contact.IdStatus >= 5)
                    {
                        var account = await _context.Accounts.SingleOrDefaultAsync(x => x.Username == Utilities.toUsername(contact.Fullname.Trim(), contact.Phone.Trim()));
                        var learn = await _context.Learns.SingleOrDefaultAsync(x => (x.IdStudent == account.IdAccount && x.IdCalendar == contact.IdCalendar));
                        var calendar = await _context.Calendars.SingleOrDefaultAsync(x => x.IdCalendar == contact.IdCalendar);

                        if (account == null || learn == null || calendar == null || calendar.Slotnow == 0)
                        {
                            _notyf.Error("Không thực hiện được");
                        }
                        else
                        {
                            _context.Learns.Remove(learn);
                            _context.Accounts.Remove(account);

                            calendar.Slotnow--;
                            _context.Calendars.Update(calendar);

                            _notyf.Success("Đã xóa tài khoản");
                            _notyf.Success("Đã xóa trong lịch học");
                        }
                    }
                }
                catch
                {

                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.IdContact == id);
        }
    }
}
