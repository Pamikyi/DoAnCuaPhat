using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SneakerShop.Data;
using SneakerShop.Services;  // ⬅ PHẢI CÓ
using SneakerShop.SessionModels;

public class CartController : Controller
{
    private readonly AppDbContext _context;
    private readonly CartService _cartService;

    public CartController(AppDbContext context, CartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }
    [HttpPost]
    [HttpPost]
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

    [HttpGet]
    public IActionResult Count()
    {
        int total = _cartService.GetCart().Sum(x => x.Quantity);
        return Json(total);
    }
    public IActionResult Cart()
    {
        var cart = _cartService.GetCart();
        return View(cart);
    }
}
