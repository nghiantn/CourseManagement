using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using PagedList.Core;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Rewrite;
using System.Security.Principal;


namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminAccountsController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Admin/Accounts
        public IActionResult Index(int page = 1, int IdRole = 0)
        {
            var pageNumber = page;
            var pageSize = 10;

            List<Account> lsAccounts = new List<Account>();

            if (IdRole != 0)
            {
                lsAccounts = _context.Accounts
                .AsNoTracking()
                .Where(x => x.IdRole == IdRole)
                .Include(x => x.IdRoleNavigation)
                .OrderBy(x => x.IdAccount).ToList();
            }
            else
            {
                lsAccounts = _context.Accounts
                .AsNoTracking()
                .Include(x => x.IdRoleNavigation)
                .OrderBy(x => x.IdAccount).ToList();
            }

            PagedList<Account> models = new PagedList<Account>(lsAccounts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.CurrentIdRole = IdRole;
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            return View(models);
        }

        // GET: Admin/Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdAccount == id);
            if (account == null)
            {
                return NotFound();
            }
            _notyf.Information("Thông tin tài khoản");
            return View(account);
        }

        // GET: Admin/Accounts/Create
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            _notyf.Information("Đang thêm mới");
            return View();
        }

        // POST: Admin/Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAccount,Username,Password,Fullname,Phone,Active,IdRole")] Account account)
        {
            if (ModelState.IsValid)
            {
                if (!AccountExists(account.Username))
                {
                    _notyf.Error("Tài khoản đã có");
                    return View();
                }
                _context.Add(account);
                await _context.SaveChangesAsync();
                _notyf.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            return View(account);
        }

        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name", account.IdRole);
            _notyf.Warning("Đang chỉnh sửa");
            return View(account);
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAccount,Username,Password,Fullname,Phone,Active,IdRole")] Account account)
        {
            if (id != account.IdAccount)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name", account.IdRole);
            _notyf.Success("Chỉnh sửa thành công");
            return View(account);
        }

        // GET: Admin/Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Accounts == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.IdRoleNavigation)
                .FirstOrDefaultAsync(m => m.IdAccount == id);
            if (account == null)
            {
                return NotFound();
            }
            _notyf.Error("Xác nhận xóa");
            return View(account);
        }

        // POST: Admin/Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Accounts == null)
            {
                return Problem("Entity set 'CourseDatabaseContext.Accounts'  is null.");
            }
            var account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            _notyf.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string Username)
        {
            //List<Account> lsAccounts = new List<Account>();

            //lsAccounts = _context.Accounts
            //.AsNoTracking()
            //    .Where(x => String.Compare(Username, x.Username) == 0)
            //    .Include(x => x.IdRoleNavigation)
            //    .OrderBy(x => x.IdAccount).ToList();
            //return (lsAccounts == null);
            return true;
        }
    }
}
