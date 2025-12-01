using Microsoft.AspNetCore.Mvc;
using SneakerShop.Data;
using SneakerShop.Helpers;
using SneakerShop.Models;
using Microsoft.EntityFrameworkCore;

public class AuthController : Controller
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // ========== REGISTER ==========
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string FullName, string Email, string Password)
    {
        //check trùng email
        if (_context.Users.Any(u => u.Email == Email))
        {
            TempData["Error"] = "Email đã tồn tại!";
            return RedirectToAction("Register");
        }

        var user = new User
        {
            FullName = FullName,
            Email = Email,
            PasswordHash = PasswordHelper.Hash(Password),
            Role = "User",
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // Tự đăng nhập sau khi đăng ký
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.FullName);

        return RedirectToAction("Login");
    }

    // ========== LOGIN ==========
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
public IActionResult Login(string Email, string Password)
{
    var user = _context.Users.FirstOrDefault(u => u.Email == Email);

    if (user == null || user.PasswordHash != Password)
    {
        TempData["Error"] = "Sai email hoặc mật khẩu!";
        return RedirectToAction("Login");
    }

    // Lưu session
    HttpContext.Session.SetInt32("UserId", user.Id);
    HttpContext.Session.SetString("UserName", user.FullName ?? "User");
    HttpContext.Session.SetString("Role", user.Role ?? "User");

    // Nếu là admin → vào admin dashboard
    if (user.Role == "Admin")
    {
        return RedirectToAction("Index", "Admin");
    }

    // Nếu là user → về trang chủ
    return RedirectToAction("Index", "Home");
}

    // ========== LOGOUT ==========
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
