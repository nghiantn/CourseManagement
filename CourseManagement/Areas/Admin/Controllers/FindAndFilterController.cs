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

        [HttpPost]
        public IActionResult FindAccounts(string keyword, int IdRole)
        {
            return View();
        }

        public IActionResult FiltterAccounts(int IdRole = 0)
        {
            var url = $"/Admin/AdminAccounts?IdRole={IdRole}";
            if (IdRole == 0)
            {
                url = $"/Admin/AdminAccounts";
            }
            return Json(new { status = "success", redirectUrl = url });
        }
    }
}
