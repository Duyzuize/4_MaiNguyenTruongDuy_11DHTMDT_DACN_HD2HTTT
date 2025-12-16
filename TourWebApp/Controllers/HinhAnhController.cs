using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class HinhAnhController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HinhAnhController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hinhAnhs = _context.HinhTours
                .Include(h => h.IdTourNavigation)
                .Where(h => h.IdTourNavigation != null && h.IdTourNavigation.TrangThai == true)
                .OrderBy(h => h.IdTour)
                .ThenBy(h => h.ThuTu)
                .AsNoTracking()
                .ToList();

            return View(hinhAnhs);
        }
    }
}
