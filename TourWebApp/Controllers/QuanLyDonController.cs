using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class QuanLyDonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyDonController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
            => HttpContext.Session.GetString("VaiTro") == "Admin";

        // ===============================
        // ▶ DANH SÁCH ĐƠN
        // ===============================
        public IActionResult Index(string trangThai = "TatCa")
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var query = _context.DonDatTours
                .Include(d => d.IdTourNavigation)
                .Include(d => d.IdTaiKhoanNavigation)
                .OrderByDescending(d => d.NgayDat)
                .AsQueryable();

            switch (trangThai)
            {
                case "ChoDuyet":
                    query = query.Where(d => d.TrangThai == "ChoDuyet");
                    break;

                case "DaDuyet":
                    query = query.Where(d => d.TrangThai == "DaDuyet");
                    break;

                case "DaHuy":
                    query = query.Where(d => d.TrangThai == "DaHuy");
                    break;

                case "DaThanhToan":
                    query = query.Where(d => d.DaThanhToan == true);
                    break;

                case "ChuaThanhToan":
                    query = query.Where(d => d.DaThanhToan == false);
                    break;
            }

            ViewBag.TrangThai = trangThai;
            return View(query.ToList());
        }


        // ===============================
        // ▶ CHI TIẾT ĐƠN
        // ===============================
        public IActionResult ChiTiet(int id)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var don = _context.DonDatTours
                .Include(d => d.IdTourNavigation)
                .Include(d => d.IdLichNavigation)
                .Include(d => d.IdTaiKhoanNavigation)
                .FirstOrDefault(d => d.IdDon == id);

            if (don == null) return NotFound();

            return View(don);
        }

        // ===============================
        // ▶ DUYỆT ĐƠN
        // ===============================
        [HttpPost]
        public IActionResult Duyet(int id)
        {
            var don = _context.DonDatTours.Find(id);
            if (don == null) return NotFound();

           
            if (don.DaThanhToan)
            {
                TempData["Error"] = "Đơn đã thanh toán, không cần duyệt.";
                return RedirectToAction("ChiTiet", new { id });
            }

            don.TrangThai = "DaDuyet";
            _context.SaveChanges();

            TempData["Success"] = "Đã duyệt đơn.";
            return RedirectToAction("ChiTiet", new { id });
        }


        // ===============================
        // ▶ HỦY ĐƠN
        // ===============================
        [HttpPost]
        public IActionResult Huy(int id, string? ghiChu)
        {
            var don = _context.DonDatTours.Find(id);
            if (don == null) return NotFound();

           
            if (don.DaThanhToan)
            {
                TempData["Error"] = "Đơn đã thanh toán, không thể hủy.";
                return RedirectToAction("ChiTiet", new { id });
            }

            don.TrangThai = "DaHuy";
            don.GhiChu = ghiChu;
            don.NgayHuy = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Đã hủy đơn.";
            return RedirectToAction("ChiTiet", new { id });
        }
        
        [HttpPost]
        public IActionResult Xoa(int id)
        {
            var don = _context.DonDatTours.Find(id);
            if (don == null) return NotFound();

            
            if (don.DaThanhToan)
            {
                TempData["Error"] = "Không thể xóa đơn đã thanh toán.";
                return RedirectToAction("Index");
            }

           
            var thongBaos = _context.ThongBaos
                .Where(tb => tb.IdDon == id)
                .ToList();

            if (thongBaos.Any())
            {
                _context.ThongBaos.RemoveRange(thongBaos);
            }

           
            _context.DonDatTours.Remove(don);

          
            _context.SaveChanges();

            TempData["Success"] = "Đã xóa đơn chưa thanh toán.";
            return RedirectToAction("Index");
        }

    }
}
