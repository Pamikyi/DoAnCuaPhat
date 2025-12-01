using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SneakerShop.Data;
using SneakerShop.Services;
using SneakerShop.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using SneakerShop.SessionModels;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly AppDbContext _context;
    private readonly CartService _cartService;
    private readonly EmailService _email;

    public CartController(AppDbContext context, CartService cartService, EmailService email)
    {
        _context = context;
        _cartService = cartService;
        _email = email;
    }

    // ===========================
    // ⭐ THÊM VÀO GIỎ
    // ===========================
    public IActionResult Add(int id)
    {
        var shoe = _context.Shoes
            .Include(s => s.ShoeImages)
            .FirstOrDefault(s => s.Id == id);

        if (shoe == null)
            return Json(new { success = false });

        var image = shoe.ShoeImages?.FirstOrDefault()?.ImageUrl ?? "/images/no-image.jpg";

        _cartService.AddToCart(shoe.Id, shoe.Name, image, shoe.Price);

        return Json(new { success = true });
    }

    // ===========================
    // ⭐ ĐẾM SỐ LƯỢNG TRONG GIỎ
    // ===========================
    [HttpGet]
    public IActionResult Count()
    {
        int total = _cartService.GetCart().Sum(x => x.Quantity);
        return Json(total);
    }

    // ===========================
    // ⭐ TRANG GIỎ HÀNG
    // ===========================
    public IActionResult Cart()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }

    // ===========================
    // ⭐ XÓA ITEM TRONG GIỎ
    // ===========================
    [Route("Cart/Remove/{id}")]
    public IActionResult Remove(int id)
    {
        _cartService.Remove(id);
        return RedirectToAction("Cart");
    }

    // ===========================
    // ⭐ CẬP NHẬT SỐ LƯỢNG
    // ===========================
    [HttpPost]
    public IActionResult UpdateQuantity(int id, int quantity)
    {
        _cartService.UpdateQuantity(id, quantity);
        return Json(new { success = true });
    }

    // ===========================
    // ⭐ CHECKOUT (GET)
    // ===========================
    [HttpGet]
    public IActionResult Checkout(string? mode)
    {
        // BUY NOW MODE
        if (mode == "buynow")
        {
            var data = HttpContext.Session.GetString("BUY_NOW_ITEM");
            if (data == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm mua ngay.";
                return RedirectToAction("Cart");
            }

            var item = JsonConvert.DeserializeObject<CartItem>(data);

            return View("Checkout", new CheckoutViewModel
            {
                IsBuyNow = true,
                BuyNowItem = item
            });
        }

        // GIỎ HÀNG BÌNH THƯỜNG
        var cart = _cartService.GetCart();
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng trống!";
            return RedirectToAction("Cart");
        }

        return View("Checkout", new CheckoutViewModel
        {
            IsBuyNow = false,
            CartItems = cart
        });
    }

    // ===========================
    // ⭐ PROCESS CHECKOUT (POST)
    // ===========================
    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(string FullName, string Email, string Address, bool IsBuyNow)
    {
        // ====================================================
        // ⭐ CASE 1: MUA NGAY
        // ====================================================
        if (IsBuyNow)
        {
            var data = HttpContext.Session.GetString("BUY_NOW_ITEM");
            if (data == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm mua ngay.";
                return RedirectToAction("Cart");
            }

            var item = JsonConvert.DeserializeObject<CartItem>(data);

            // Gửi email
            string adminEmail = "1901duy@gmail.com";
            string subject = "Đơn hàng (Mua Ngay)";
            string body = $@"
                <h3>Khách mua ngay</h3>
                <p><b>Họ tên:</b> {FullName}</p>
                <p><b>Email:</b> {Email}</p>
                <p><b>Địa chỉ:</b> {Address}</p>

                <h3>Chi tiết sản phẩm:</h3>
                {item.Name} - SL: 1 - Giá: {item.Total:N0}₫
            ";

            await _email.SendEmailAsync(adminEmail, subject, body);

            // Lưu đơn hàng
            var order = new Order
            {
                UserId = 1,
                TotalAmount = item.Total,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            _context.OrderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                ShoeId = item.ShoeId,
                Quantity = 1,
                UnitPrice = item.UnitPrice
            });

            _context.SaveChanges();

            // Xóa session Buy Now
            HttpContext.Session.Remove("BUY_NOW_ITEM");

            return RedirectToAction("Success");
        }


        // ====================================================
        // ⭐ CASE 2: CHECKOUT GIỎ HÀNG
        // ====================================================
        var cart = _cartService.GetCart();
        if (!cart.Any())
        {
            TempData["Error"] = "Giỏ hàng trống!";
            return RedirectToAction("Cart");
        }

        // Gửi email
        string adminEmailNormal = "1901duy@gmail.com";
        string subjectNormal = "Đơn hàng mới từ khách hàng";

        string bodyNormal = $@"
            <h3>Thông tin khách hàng</h3>
            <p><b>Họ tên:</b> {FullName}</p>
            <p><b>Email:</b> {Email}</p>
            <p><b>Địa chỉ:</b> {Address}</p>

            <h3>Chi tiết đơn hàng:</h3>
            {string.Join("<br>", cart.Select(c => $"{c.Name} - SL: {c.Quantity} - Giá: {c.Total:N0}₫"))}";

        await _email.SendEmailAsync(adminEmailNormal, subjectNormal, bodyNormal);

        // Lưu đơn hàng
        var newOrder = new Order
        {
            UserId = 1,
            TotalAmount = cart.Sum(i => i.Total),
            Status = "Pending",
            CreatedAt = DateTime.Now
        };

        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        foreach (var item in cart)
        {
            _context.OrderItems.Add(new OrderItem
            {
                OrderId = newOrder.Id,
                ShoeId = item.ShoeId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }

        _context.SaveChanges();

        // Xóa giỏ hàng
        _cartService.ClearCart();

        return RedirectToAction("Success");
    }

    // ===========================
    // ⭐ SUCCESS PAGE
    // ===========================
    public IActionResult Success()
    {
        return View();
    }

    // ===========================
    // ⭐ MUA NGAY
    // ===========================
    [HttpPost]
    public IActionResult BuyNow(int id)
    {
        var shoe = _context.Shoes
            .Include(s => s.ShoeImages)
            .FirstOrDefault(s => s.Id == id);

        if (shoe == null)
            return Json(new { success = false });

        var image = shoe.ShoeImages?.FirstOrDefault()?.ImageUrl ?? "/images/no-image.jpg";

        var buyNowItem = new CartItem
        {
            ShoeId = shoe.Id,
            Name = shoe.Name,
            Image = image,
            Quantity = 1,
            UnitPrice = shoe.Price
        };

        HttpContext.Session.SetString("BUY_NOW_ITEM", JsonConvert.SerializeObject(buyNowItem));

        return Json(new { success = true, redirect = "/Cart/Checkout?mode=buynow" });
    }
}
