using AspNetCoreHero.ToastNotification.Abstractions;
using CourseManagement.Areas.Admin.Models;
using CourseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminDashboardController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminDashboardController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("IdAccount");
            if (taikhoanID != null)
            {
                var khachhang = _context.Accounts.AsNoTracking()
                    .Include(x=> x.IdRoleNavigation)
                    .SingleOrDefault(x => x.IdAccount == Convert.ToInt32(taikhoanID));

                if (khachhang != null)
                {
                    var res = new Regex("\\S").Replace(khachhang.Password, "*");
                    ViewBag.Pass = res;

                    ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");

                    return View(khachhang);
                }
            }

            _notyf.Warning("Vui lòng đăng nhập lại");
            return RedirectToAction("Logout", "AdminAccounts", new { Area = "Admin" });
        }
    }
}
