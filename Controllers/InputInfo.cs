using Microsoft.AspNetCore.Mvc;
using SneakerShop.Data;
using SneakerShop.Models;
using Microsoft.EntityFrameworkCore;

public class InputInfoController : Controller
{
    private readonly AppDbContext _context;

        // Inject DbContext
        public InputInfoController(AppDbContext context)
        {
            _context = context;
        }
    public IActionResult InputInfo()
    {
        return View();
    }

    [HttpPost]
    public IActionResult InputInfo(string Phone, string Address)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
            return RedirectToAction("Login", "Account");

        user.Phone = Phone;
        user.Address = Address;
        _context.SaveChanges();

        TempData["Success"] = "Cập nhật thông tin thành công!";
        return RedirectToAction("ProcessCheckout");
    }
}