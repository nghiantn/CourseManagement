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
using PagedList.Core;

namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class AdminRolesController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminRolesController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Admin/AdminRoles
        public IActionResult Index(int page = 1)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Role> lsRoles = new List<Role>();

            lsRoles = _context.Roles
            .AsNoTracking()
                .OrderBy(x => x.IdRole).ToList();

            PagedList<Role> models = new PagedList<Role>(lsRoles.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsRoles.Count / (Double)pageSize);

            return View(models);
        }



        public IActionResult Create()
        {
            _notyf.Information("Đang thêm mới");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRole,Name,Description")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                _notyf.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            _notyf.Warning("Đang chỉnh sửa");
            return View(role);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRole,Name,Description")] Role role)
        {
            if (id != role.IdRole)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.IdRole))
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
            return View(role);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.IdRole == id);
            if (role == null)
            {
                return NotFound();
            }
            _notyf.Error("Xác nhận xóa");
            return View(role);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Roles == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Roles'  is null.");
            }
            var role = await _context.Roles.FindAsync(id);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
            
            await _context.SaveChangesAsync();
            _notyf.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool RoleExists(int id)
        {
          return _context.Roles.Any(e => e.IdRole == id);
        }
    }
}
