using Microsoft.AspNetCore.Mvc;
using SneakerShop.Data;
using SneakerShop.Models;

public class UserController : Controller
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    // ===========================
    // HIỂN THỊ TRANG PROFILE
    // ===========================
    public IActionResult Profile()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var user = _db.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
            return RedirectToAction("Login", "Auth");

        return View(user);
    }

    // ===========================
    // CẬP NHẬT THÔNG TIN PROFILE
    // ===========================
    [HttpPost]
    public IActionResult UpdateProfile(int Id, string FullName, string Phone, string Address)
    {
        var user = _db.Users.FirstOrDefault(u => u.Id == Id);

        if (user == null)
            return RedirectToAction("Login", "Auth");

        // Cập nhật
        user.FullName = FullName;
        user.Phone = Phone;
        user.Address = Address;

        _db.SaveChanges();

        TempData["Success"] = "Cập nhật thành công!";
        return RedirectToAction("Profile");
    }
}
