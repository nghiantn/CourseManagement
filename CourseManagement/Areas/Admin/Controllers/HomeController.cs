using AspNetCoreHero.ToastNotification.Abstractions;
using CourseManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManagement.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        //[Authorize]
        //[Route("admin.khoahoc", Name = "Index")]
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
