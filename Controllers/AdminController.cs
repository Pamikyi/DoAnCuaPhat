using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Admin()
    {
        // Check quyền Admin
        string? role = HttpContext.Session.GetString("Role");

        if (role != "Admin")
            return RedirectToAction("Login", "Auth");

        return View();
    }
    [AdminAuthorize]

    public IActionResult Index()
    {
        return View();
    }
}


