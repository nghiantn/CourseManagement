using AspNetCoreHero.ToastNotification.Abstractions;
using CourseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly CourseDatabaseContext _context;
        private readonly ILogger<HomeController> _logger;
        public INotyfService _notyf;

        public HomeController(CourseDatabaseContext context, ILogger<HomeController> logger, INotyfService notyf)
        {
            _context = context;
            _logger = logger;
            _notyf = notyf;
        }

        public IActionResult Index()
        {
            //_notyf.Custom("Custom Notification - closes in 5 seconds.", 5, "whitesmoke", "fa fa-gear");
            //_notyf.Custom("Custom Notification - closes in 10 seconds.", 10, "#B600FF", "fa fa-home");
            var courseDatabaseContext = _context.Courses.Include(c => c.IdCategoryNavigation);
            ViewBag.course = courseDatabaseContext;

            return View();
        }

        public IActionResult Privacy()
        {
            _notyf.Information("Chào");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Home/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(a => a.IdCategoryNavigation)
                .FirstOrDefaultAsync(m => m.IdCourse == id);

            ViewBag.course = course;

            return View();
        }

        // GET: Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: Home/Courses
        public IActionResult Courses()
        {
            return View();
        }

        // GET: Home/Contact
        public IActionResult Contact()
        {
            return View();
        }

        // GET: Home/Login
        public IActionResult Login()
        {
            return View();
        }

        // GET: Home/Register
        public IActionResult Register()
        {
            return View();
        }

        // GET: Home/Cart
        public IActionResult Cart()
        {
            return View();
        }
    }
}
