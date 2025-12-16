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
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("DangNhap", "TaiKhoan");

            var ds = _context.BinhLuanTours
                .Include(x => x.IdBaiVietNavigation)
                .Include(x => x.IdTaiKhoanNavigation)
                .OrderByDescending(x => x.NgayBL)
                .ToList();

            return View(ds);
        }

        // ======================
        // ADMIN PHẢN HỒI
        // ======================
        [HttpPost]
        public IActionResult PhanHoi(int id, string phanHoi)
        {
            if (!IsAdmin())
                return RedirectToAction("DangNhap", "TaiKhoan");

            var bl = _context.BinhLuanTours.FirstOrDefault(x => x.IdBL == id);
            if (bl != null)
            {
                // ✅ Lưu phản hồi admin
                bl.PhanHoiAdm = phanHoi;

                // ✅ Tự bật hiển thị sau khi admin duyệt
                bl.HienThi = true;

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ======================
        // ẨN / HIỆN BÌNH LUẬN
        // ======================
        [HttpPost]
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
