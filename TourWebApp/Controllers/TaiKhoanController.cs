using Microsoft.AspNetCore.Mvc;
using TourWebApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Controllers
{
    public class TaiKhoanController : Controller
    {

        private readonly ApplicationDbContext _context;

        public TaiKhoanController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= ĐĂNG KÝ =================
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(string HoTen, string Email, string MatKhau, string MatKhauNhapLai)
        {
            if (MatKhau != MatKhauNhapLai)
            {
                ViewBag.ThongBao = "❌ Mật khẩu không khớp";
                return View();
            }

            // Kiểm tra trùng email
            if (_context.TaiKhoans.Any(x => x.Email == Email))
            {
                ViewBag.ThongBao = "❌ Email đã tồn tại";
                return View();
            }

            string vaiTro = "User";

            if (Email.EndsWith("@happydulich.vn"))
                vaiTro = "Admin";
            else if (!Email.EndsWith("@gmail.com"))
            {
                ViewBag.ThongBao = "❌ Email phải là gmail hoặc happydulich.vn";
                return View();
            }

            TaiKhoan tk = new TaiKhoan
            {
                HoTen = HoTen,
                Email = Email,
                MatKhau = MatKhau,
                VaiTro = vaiTro,
                TrangThai = true,
                NgayTao = DateTime.Now
            };

            _context.TaiKhoans.Add(tk);
            _context.SaveChanges();

            ViewBag.ThongBao = "✅ Đăng ký thành công! Hãy đăng nhập";
            return View();
        }

        // ================= ĐĂNG NHẬP =================
       [HttpGet]
        public IActionResult DangNhap(string? returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public IActionResult DangNhap(string Email, string MatKhau, string? returnUrl)
        {
            var user = _context.TaiKhoans
                .FirstOrDefault(x => x.Email == Email && x.MatKhau == MatKhau && x.TrangThai == true);

            if (user == null)
            {
                ViewBag.ThongBao = "❌ Sai email hoặc mật khẩu";
                return View();
            }

            // Lưu session
            HttpContext.Session.SetInt32("IdTaiKhoan", user.IdTaiKhoan);
            HttpContext.Session.SetString("HoTen", user.HoTen);
            HttpContext.Session.SetString("VaiTro", user.VaiTro);

            if (!string.IsNullOrEmpty(returnUrl))
            return Redirect(returnUrl);

            // ⭐ ADMIN luôn ưu tiên về Dashboard trước ⭐
            if (user.VaiTro == "Admin")
            {
                return RedirectToAction("Dashboard", "QuanTri");
            }

            // ⭐ Với user thường → mới xử lý ReturnUrl ⭐
            if (TempData["ReturnUrl"] != null)
            {
                string? url = TempData["ReturnUrl"] as string;
                if (!string.IsNullOrEmpty(url))
                    return Redirect(url);
            }

            // ⭐ User bình thường → về Home
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult HoSo()
        {
            var id = HttpContext.Session.GetInt32("IdTaiKhoan");
            if (id == null) return RedirectToAction("DangNhap");

            var user = _context.TaiKhoans.FirstOrDefault(x => x.IdTaiKhoan == id);

            // ✅ LẤY DANH SÁCH ĐƠN CỦA USER
            var donCuaToi = _context.DonDatTours
                .Include(d => d.IdTourNavigation)
                .Where(d => d.IdTaiKhoan == id)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            ViewBag.DonCuaToi = donCuaToi;

            return View(user);
        }


        [HttpPost]
        public IActionResult HoSo(TaiKhoan model)
        {
            var user = _context.TaiKhoans.FirstOrDefault(x => x.IdTaiKhoan == model.IdTaiKhoan);
            if (user == null) return NotFound();

            user.HoTen = model.HoTen;
            user.SoDienThoai = model.SoDienThoai;
            user.DiaChi = model.DiaChi;

            _context.SaveChanges();
            ViewBag.ThongBao = "✅ Cập nhật hồ sơ thành công";
            return View(user);
        }

        public IActionResult ChiTietDon(int idDon)
        {
            int? userId = HttpContext.Session.GetInt32("IdTaiKhoan");
            if (userId == null)
                return RedirectToAction("DangNhap");

           var don = _context.DonDatTours
            .Include(d => d.IdTourNavigation)
            .Include(d => d.IdLichNavigation)
            .FirstOrDefault(d => d.IdDon == idDon && d.IdTaiKhoan == userId);


            if (don == null)
                return RedirectToAction("DonCuaToi");

            return View(don);
        }

        public IActionResult DonCuaToi()
        {
            int? userId = HttpContext.Session.GetInt32("IdTaiKhoan");
            if (userId == null)
                return RedirectToAction("DangNhap");

            var dsDon = _context.DonDatTours
                .Include(d => d.IdTourNavigation)
                .Include(d => d.IdLichNavigation)
                .Where(d => d.IdTaiKhoan == userId)
                .OrderByDescending(d => d.NgayDat)
                .ToList();

            return View(dsDon);
        }


        // ================= ĐĂNG XUẤT =================
        public IActionResult DangXuat()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("DangNhap");
        }

    }
}
