using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class QuanLyChuyenMucController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuanLyChuyenMucController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ CHECK ADMIN
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        // ================= DANH SÁCH =================
        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var ds = _context.ChuyenMucBaiViets
                .OrderByDescending(x => x.IdCM)
                .ToList();

            return View(ds);
        }

        // ================= CREATE =================
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChuyenMucBaiViet cm)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            if (!ModelState.IsValid)
                return View(cm);

            // nếu con không tick gì ở view thì TrangThai default true đã set ở DB
            _context.ChuyenMucBaiViets.Add(cm);
            _context.SaveChanges();   // ✅ LƯU VÀO CSDL

            return RedirectToAction(nameof(Index));
        }

        // ================= EDIT =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var cm = _context.ChuyenMucBaiViets.Find(id);
            if (cm == null) return NotFound();

            return View(cm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ChuyenMucBaiViet cm)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            if (!ModelState.IsValid)
                return View(cm);

            _context.ChuyenMucBaiViets.Update(cm);
            _context.SaveChanges();   // ✅ UPDATE CSDL

            return RedirectToAction(nameof(Index));
        }

        // ================= DELETE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var cm = _context.ChuyenMucBaiViets.Find(id);
            if (cm != null)
            {
                _context.ChuyenMucBaiViets.Remove(cm);
                _context.SaveChanges();   // ✅ XÓA CSDL
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
