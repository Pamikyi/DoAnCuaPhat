using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SneakerShop.Data;
using System.Threading.Tasks;

namespace SneakerShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Detail(int id)
        {
            var product = _context.Shoes
                .Include(s => s.ShoeImages)
                .Include(s => s.Category)
                    .ThenInclude(c => c.Shoes)
                        .ThenInclude(s => s.ShoeImages)
                .Include(s => s.ShoeSizes)
                .FirstOrDefault(s => s.Id == id);

            return View(product);
        }

    }
}
