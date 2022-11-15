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
using CourseManagement.Extension;
using CourseManagement.Areas.Admin.Models;


namespace CourseManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class AdminAccountsController : Controller
    {
        private readonly CourseDatabaseContext _context;
        public INotyfService _notyf;

        public AdminAccountsController(CourseDatabaseContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

       
        public IActionResult Index(int IdRole = 0, int page = 1)
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
            ViewBag.PageLength = (int)Math.Ceiling((Double)lsAccounts.Count / (Double)pageSize);
            ViewBag.CurrentIdRole = IdRole;
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            return View(models);
        }
       
        public IActionResult Create()
        {
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            _notyf.Information("Đang thêm mới");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAccount,Username,Password,Fullname,Phone,Active,IdRole")] Account account)
        {

            if (ModelState.IsValid)
            {
                if (AccountExists(account.Username))
                {
                    _notyf.Error("Tài khoản đã được sử dụng");
                    ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
                    return View();
                }
                if (account.IdRole == 0 || account.IdRole == null || account.Fullname == null || account.Phone == null)
                {
                    _notyf.Error("Chưa nhập đầy đủ thông tin");
                    ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
                    return View();
                }

                account.Password = account.Password.Trim().ToMD5();

                _context.Add(account);
                await _context.SaveChangesAsync();
                _notyf.Success("Tạo mới thành công");
                return RedirectToAction(nameof(Index));
            }

            _notyf.Error("Chưa nhập đầy đủ thông tin");
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            return View(account);
        }

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
                    if (account.IdRole == 0 || account.IdRole == null || account.Fullname == null || account.Phone == null)
                    {
                        _notyf.Error("Chưa nhập đầy đủ thông tin");
                        ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
                        return View();
                    }

                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _notyf.Error("Vui lòng thử lại");
                    ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
                    return View();
                }
                _notyf.Success("Chỉnh sửa thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name", account.IdRole);
            return View(account);
        }

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
            List<Account> lsAccounts = new List<Account>();

            lsAccounts = _context.Accounts
            .AsNoTracking()
                .Where(x => x.Username.Trim() == Username.Trim())
                .OrderBy(x => x.IdAccount).ToList();
            if (lsAccounts.Count > 0) return true;
            return false;
        }

        public async Task<IActionResult> ChangePassword(int? id)
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
            _notyf.Information("Đang đổi mật khẩu");
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            return View(account);
        }

        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int id, [Bind("IdAccount,Username,Password,Fullname,Phone,Active,IdRole")] Account account)
        {
            if (id != account.IdAccount)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (account.Password == null)
                    {
                        _notyf.Error("Chưa nhập mật khẩu");
                        ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
                        return View();
                    }

                    account.Password = account.Password.Trim().ToMD5();

                    _context.Update(account);
                    _notyf.Success("Đổi mật khẩu thành công");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    _notyf.Error("Vui lòng thử lại");
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRole"] = new SelectList(_context.Roles, "IdRole", "Name");
            return View(account);
        }


        [AllowAnonymous]
        [Route("/dang-nhap.html", Name = "Login")]
        public IActionResult Login(string returnUrl = null)
        {
            var khID = HttpContext.Session.GetString("IdAccount");
            if (khID != null) return RedirectToAction("Index", "AdminDashboard", new { Area = "Admin" });
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/dang-nhap.html", Name = "Login")]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var kh = _context.Accounts.Include(x => x.IdRoleNavigation)
                        .SingleOrDefault(x => x.Username.ToLower() == model.UserName.ToLower().Trim());

                    if (kh == null)
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        _notyf.Warning("Kiểm tra lại tài khoản");
                        return View(model);
                    }

                    string pass = model.Password.Trim().ToMD5();

                    if (pass.Trim() != kh.Password.Trim())
                    {
                        ViewBag.Error = "Thông tin đăng nhập chưa chính xác";
                        _notyf.Warning("Kiểm tra lại mật khẩu");
                        return View(model);
                    }

                    if (!kh.Active)
                    {
                        ViewBag.Error = "Tài khoản không được phép hoạt động";
                        _notyf.Error("Tài khoản bị vô hiệu");
                        return View(model);
                    }

                    _context.Update(kh);
                    await _context.SaveChangesAsync();

                    var khID = HttpContext.Session.GetString("IdAccount");

                    HttpContext.Session.SetString("IdAccount", kh.IdAccount.ToString());

                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, kh.Fullname),
                        new Claim("IdAccount", kh.IdAccount.ToString()),
                        new Claim("IdRole", kh.IdRole.ToString()),
                        new Claim(ClaimTypes.Role, kh.IdRoleNavigation.Name)
                    };
                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
                    await HttpContext.SignInAsync(userPrincipal);

                    _notyf.Success("Đăng nhập thành công");
                    return RedirectToAction("Index", "AdminDashboard", new { Area = "Admin" });
                }
            }
            catch
            {
                return RedirectToAction("Login", "AdminAccounts", new { Area = "Admin" });
            }

            ViewBag.Error = "Vui lòng nhập thông tin đăng nhập lại";
            return View(model); ;
        }

        [AllowAnonymous]
        [Route("/dang-xuat.html", Name = "Logout")]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync();
                HttpContext.Session.Remove("IdAccount");
                _notyf.Warning("Bạn đã đăng xuất");
                return RedirectToAction("Login", "AdminAccounts", new { Area = "Admin" });
            }
            catch
            {
                _notyf.Warning("Bạn đã đăng xuất");
                return RedirectToAction("Login", "AdminAccounts", new { Area = "Admin" });
            }
        }
    }
}

