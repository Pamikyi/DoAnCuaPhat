using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SneakerShop.Data;
using System.Threading.Tasks;

public class AnotherPageController : Controller
{
    public IActionResult About()
    {
        return View("~/Views/AnotherPage/About.cshtml");
    }
    [Route("AnotherPage/Contact")]
    public IActionResult Contact()
    {
        return View("~/Views/AnotherPage/Contact.cshtml");
    }

    private readonly EmailService _emailService;

    public AnotherPageController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> SendContact(string FullName, string Email, string Message)
    {
        string adminEmail = "1901duy@gmail.com"; // email nhận liên hệ

        string subject = "Liên hệ mới từ khách hàng";
        string body = $@"
            <h3>Thông tin liên hệ</h3>
            <p><b>Họ tên:</b> {FullName}</p>
            <p><b>Email:</b> {Email}</p>
            <p><b>Nội dung:</b> {Message}</p>
        ";

        await _emailService.SendEmailAsync(adminEmail, subject, body);

        TempData["Success"] = "Gửi liên hệ thành công! Chúng tôi sẽ phản hồi sớm nhất.";

        return RedirectToAction("Contact");  // quay lại trang Contact
    }
}
