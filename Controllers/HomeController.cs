using Microsoft.AspNetCore.Mvc;
using SneakerShop.Data;
using SneakerShop.Models;
using Microsoft.EntityFrameworkCore;

namespace SneakerShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        // Inject DbContext
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var vm = new HomeViewModel
            {
                AllShoes = _context.Shoes
                    .Include(s => s.ShoeImages)
                    .ToList(),

                Running = _context.Shoes
                    .Where(s => s.CategoryId == 1)
                    .Include(s => s.ShoeImages)
                    .ToList(),

                Training = _context.Shoes
                    .Where(s => s.CategoryId == 2)
                    .Include(s => s.ShoeImages)
                    .ToList(),

                Lifestyle = _context.Shoes
                    .Where(s => s.CategoryId == 3)
                    .Include(s => s.ShoeImages)
                    .ToList()
            };

            return View(vm);
        }
    }
}
