using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class QuanLyBinhLuanController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyBinhLuanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // CHECK ADMIN
        // ======================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        // ======================
        // DANH SÁCH BÌNH LUẬN
        // ======================
        public IActionResult Index(string? loai)
        {
            if (!IsAdmin())
                return RedirectToAction("DangNhap", "TaiKhoan");

            var query = _context.BinhLuanTours
                .Include(x => x.IdTourNavigation)
                .Include(x => x.IdBaiVietNavigation)
                .Include(x => x.IdTaiKhoanNavigation)
                .AsQueryable();

            if (loai == "tour") query = query.Where(x => x.IdTour != null);
            else if (loai == "baiviet") query = query.Where(x => x.IdBaiViet != null);

            ViewBag.Loai = loai;

            return View(query.OrderByDescending(x => x.NgayBL).ToList());
        }


        // ======================
        // ADMIN PHẢN HỒI
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PhanHoi(int id, string phanHoi)
        {
            if (!IsAdmin())
                return RedirectToAction("DangNhap", "TaiKhoan");

            var bl = _context.BinhLuanTours.FirstOrDefault(x => x.IdBL == id);
            if (bl != null)
            {
                bl.PhanHoiAdm = phanHoi;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ======================
        // ẨN / HIỆN BÌNH LUẬN
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id)
        {
            if (!IsAdmin())
                return RedirectToAction("DangNhap", "TaiKhoan");

            var bl = _context.BinhLuanTours.FirstOrDefault(x => x.IdBL == id);
            if (bl != null)
            {
                bl.HienThi = !bl.HienThi;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
