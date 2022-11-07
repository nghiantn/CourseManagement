using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FindAndFilterController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyfService { get; }
        public FindAndFilterController(CourseDatabaseContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        //////////////////////////////////////////////////////////////
        //////Find And Filter Accounts////////////////////////////////
        //////////////////////////////////////////////////////////////
        public IActionResult FiltterAccounts(int IdRole = 0)
        {
            var url = $"/Admin/AdminAccounts?IdRole={IdRole}&Page=1";
            if (IdRole == 0)
            {
                url = $"/Admin/AdminAccounts?IdRole=0&Page=1";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        //////////////////////////////////////////////////////////////
        //////Find And Filter Courses////////////////////////////////
        //////////////////////////////////////////////////////////////
        public IActionResult FiltterCourses(int IdCategory = 0)
        {
            var url = $"/Admin/AdminCourses?IdCategory={IdCategory}&Page=1";
            if (IdCategory == 0)
            {
                url = $"/Admin/AdminCourses?IdCategory=0&Page=1";
            }
            return Json(new { status = "success", redirectUrl = url });
        }

        //////////////////////////////////////////////////////////////
        //////Find And Filter Calendars////////////////////////////////
        //////////////////////////////////////////////////////////////
        public IActionResult FiltterCalendars(int IdCourse = 0)
        {
            var url = $"/Admin/AdminCalendars?IdCourse={IdCourse}&Page=1";
            if (IdCourse == 0)
            {
                url = $"/Admin/AdminCalendars?IdCourse=0&Page=1";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
