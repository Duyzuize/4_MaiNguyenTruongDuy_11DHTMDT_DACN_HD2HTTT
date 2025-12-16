using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===============================
        // DANH SÁCH BÀI VIẾT
        // ===============================
        public IActionResult Index()
        {
            var baiViets = _context.BaiViets
                .Where(x => x.TrangThai == true)
                .OrderByDescending(x => x.NgayDang)
                .ToList();

            return View(baiViets);
        }

        // ===============================
        // CHI TIẾT BÀI VIẾT
        // ===============================
        public IActionResult ChiTiet(int id)
        {
            var baiViet = _context.BaiViets
                .Include(x => x.IdCMNavigation)
                .FirstOrDefault(x => x.IdBaiViet == id && x.TrangThai == true);

            if (baiViet == null)
                return NotFound();

            // ✅ Lấy bình luận của bài viết
            var binhLuans = _context.BinhLuanTours
                .Where(x => x.IdBaiViet == id && x.HienThi == true)
                .OrderByDescending(x => x.NgayBL)
                .ToList();

            ViewBag.BinhLuans = binhLuans;

            // ✅ Check đăng nhập
            ViewBag.DaDangNhap = HttpContext.Session.GetInt32("IdTaiKhoan") != null;

            return View(baiViet);
        }

        // ===============================
        // THÊM BÌNH LUẬN
        // ===============================
        [HttpPost]
        public IActionResult ThemBinhLuan(int idBaiViet, string noiDung)
        {
            int? idTaiKhoan = HttpContext.Session.GetInt32("IdTaiKhoan");

            // ✅ CHƯA ĐĂNG NHẬP → CHUYỂN ĐẾN ĐĂNG NHẬP ĐÚNG CONTROLLER
            if (idTaiKhoan == null)
            {
                TempData["ReturnUrl"] = Url.Action("ChiTiet", "Blog", new { id = idBaiViet });
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            if (string.IsNullOrWhiteSpace(noiDung))
            {
                return RedirectToAction("ChiTiet", new { id = idBaiViet });
            }

            // ✅ Lấy tài khoản
            var taiKhoan = _context.TaiKhoans
                .FirstOrDefault(x => x.IdTaiKhoan == idTaiKhoan);

            var binhLuan = new BinhLuanTour
            {
                IdBaiViet = idBaiViet,
                IdTaiKhoan = idTaiKhoan,
                Ten = taiKhoan?.HoTen ?? "Người dùng",
                Email = taiKhoan?.Email,
                NoiDung = noiDung,
                NgayBL = DateTime.Now,
                HienThi = true   // portal đơn giản → hiện luôn
            };

            _context.BinhLuanTours.Add(binhLuan);
            _context.SaveChanges();

            return RedirectToAction("ChiTiet", new { id = idBaiViet });
        }

    }
}
