using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class QuanLyBaiVietController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyBaiVietController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ CHECK ADMIN
        private bool IsAdmin()
            => HttpContext.Session.GetString("VaiTro") == "Admin";

        // =====================
        // DANH SÁCH BÀI VIẾT
        // =====================
        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var ds = _context.BaiViets
                .Include(x => x.IdCMNavigation)
                .OrderByDescending(x => x.NgayDang)
                .ToList();

            return View(ds);
        }

        // =====================
        // TẠO BÀI VIẾT
        // =====================
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            ViewBag.ChuyenMucs = _context.ChuyenMucBaiViets.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(BaiViet bv, IFormFile? uploadHinh)
        {
            if (!IsAdmin()) 
                return RedirectToAction("DangNhap", "TaiKhoan");

            // ✅ LẤY ID ADMIN ĐANG LOGIN
            int? adminId = HttpContext.Session.GetInt32("IdTaiKhoan");
            if (adminId == null)
                return RedirectToAction("DangNhap", "TaiKhoan");

            // ✅ Upload ảnh
            if (uploadHinh != null && uploadHinh.Length > 0)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/blog");
                Directory.CreateDirectory(folder);

                string fileName = DateTime.Now.Ticks + "_" + uploadHinh.FileName;
                string path = Path.Combine(folder, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                uploadHinh.CopyTo(stream);

                bv.HinhAnh = "img/blog/" + fileName;
            }

            // ✅ GÁN CÁC FIELD BẮT BUỘC
            bv.IdTaiKhoan = adminId.Value;          // ⭐ DÒNG QUAN TRỌNG
            bv.NgayDang = DateTime.Now;
            bv.TrangThai = true;
            bv.TacGia = HttpContext.Session.GetString("HoTen");

            _context.BaiViets.Add(bv);
            _context.SaveChanges(); // ✅ HẾT LỖI

            return RedirectToAction("Index");
        }


        // =====================
        // ẨN / HIỆN BÀI VIẾT
        // =====================
        [HttpPost]
        public IActionResult ToggleTrangThai(int id)
        {
            var bv = _context.BaiViets.Find(id);
            if (bv != null)
            {
                bv.TrangThai = !bv.TrangThai;
                _context.SaveChanges(); // ✅
            }
            return RedirectToAction("Index");
        }
    }
}
