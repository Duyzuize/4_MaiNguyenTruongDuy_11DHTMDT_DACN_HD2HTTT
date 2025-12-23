using Microsoft.AspNetCore.Mvc;
using TourWebApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Models.ViewModels;

namespace TourWebApp.Controllers
{
    public class AdminLoaiTourController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminLoaiTourController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("VaiTro") == "Admin";
        }

        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var ds = _context.LoaiTours
                .Select(l => new LoaiTourListVM
                {
                    IdLoaiTour = l.IdLoaiTour,
                    TenLoai = l.TenLoai,
                    MoTa = l.MoTa,
                    SoTour = _context.Tours.Count(t => t.IdLoaiTour == l.IdLoaiTour)
                })
                .OrderBy(x => x.IdLoaiTour)
                .ToList();

            return View(ds);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");
            return View();
        }

        [HttpPost]
        public IActionResult Create(LoaiTour model)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(model.TenLoai))
            {
                ModelState.AddModelError("TenLoai", "Tên loại không được để trống");
                return View(model);
            }

            model.TenLoai = model.TenLoai.Trim();
            model.MoTa = model.MoTa?.Trim();

            _context.LoaiTours.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var loai = _context.LoaiTours.FirstOrDefault(x => x.IdLoaiTour == id);
            if (loai == null) return NotFound();

            return View(loai);
        }

        [HttpPost]
        public IActionResult Edit(LoaiTour model)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            if (string.IsNullOrWhiteSpace(model.TenLoai))
            {
                ModelState.AddModelError("TenLoai", "Tên loại không được để trống");
                return View(model);
            }

            var loai = _context.LoaiTours.FirstOrDefault(x => x.IdLoaiTour == model.IdLoaiTour);
            if (loai == null) return NotFound();

            loai.TenLoai = model.TenLoai.Trim();
            loai.MoTa = model.MoTa?.Trim();

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("DangNhap", "TaiKhoan");

            var loai = _context.LoaiTours.FirstOrDefault(x => x.IdLoaiTour == id);
            if (loai == null) return NotFound();

            _context.LoaiTours.Remove(loai);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
