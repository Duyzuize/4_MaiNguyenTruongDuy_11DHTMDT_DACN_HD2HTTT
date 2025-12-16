using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models; // đúng namespace context + entity

namespace TourWebApp.Controllers
{
    public class DbTestController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DbTestController(ApplicationDbContext db) => _db = db;

        // GET /db/ping (đã ok)
        [HttpGet("/db/ping")]
        public async Task<IActionResult> Ping()
        {
            var ok = await _db.Database.CanConnectAsync();
            return Ok(new { canConnect = ok, provider = _db.Database.ProviderName });
        }

        // GET /db/tours -> xem 5 dòng đầu
        [HttpGet("/db/tours")]
        public async Task<IActionResult> Tours()
        {
            var list = await _db.Tours.AsNoTracking().Take(5).ToListAsync();
            return Ok(list);
        }

        // GET /db/count -> đếm số dòng bảng Tours (đỡ sợ bảng rỗng)
        [HttpGet("/db/count")]
        public async Task<IActionResult> Count()
        {
            var total = await _db.Tours.AsNoTracking().CountAsync();
            return Ok(new { total });
        }
    }
}
