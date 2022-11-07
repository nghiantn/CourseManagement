using AspNetCoreHero.ToastNotification.Abstractions;
using CourseManagement.Areas.Admin.Models;
using CourseManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
   
    public class HomeController : Controller
    { 
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public HomeController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        [Route("admin.khoahoc", Name = "Index")]
        public IActionResult Index()
        {
            var khID = HttpContext.Session.GetString("IdAccount");

            if (khID == null)
            {
                _notyf.Warning("Vui lòng đăng nhập lại");
                return RedirectToAction("Logout", "AdminAccounts", new { Area = "Admin" });
            }

            return View();
        }

        [AllowAnonymous]
        [Route("admin.loi", Name = "Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
