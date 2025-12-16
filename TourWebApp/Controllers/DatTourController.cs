using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;
using TourWebApp.Models.ViewModels;

namespace TourWebApp.Controllers
{
    public class DatTourController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DatTourController(ApplicationDbContext db)
        {
            _db = db;
        }

        
        [HttpGet]
        public IActionResult NhapThongTin(int idTour, int idLich, int adult = 1, int child = 0, int baby = 0)
        {
            // BẮT BUỘC ĐĂNG NHẬP
            int? userId = HttpContext.Session.GetInt32("IdTaiKhoan");
            if (userId == null)
            {
                TempData["ReturnUrl"] =
                    $"/DatTour/NhapThongTin?idTour={idTour}&idLich={idLich}&adult={adult}&child={child}&baby={baby}";
                TempData["Error"] = "Vui lòng đăng nhập để đặt tour!";
                return RedirectToAction("DangNhap", "TaiKhoan");
            }

            ViewBag.User = _db.TaiKhoans.FirstOrDefault(x => x.IdTaiKhoan == userId);

            var tour = _db.Tours
                          .Include(t => t.TourGiaChiTiets)
                          .FirstOrDefault(t => t.IdTour == idTour);
            var lich = _db.LichKhoiHanhs.Find(idLich);

            if (tour == null || lich == null)
            {
                TempData["Error"] = "Không tìm thấy tour hoặc lịch khởi hành.";
                return RedirectToAction("Index", "Home");
            }

          

            decimal giaNL = (decimal)(tour.GiaKhuyenMai ?? tour.GiaGoc ?? 0);

            decimal giaTE = (decimal)(tour.TourGiaChiTiets
                .FirstOrDefault(x => x.DoiTuong == "Trẻ em")?.Gia ?? 0);

            decimal giaEB = (decimal)(tour.TourGiaChiTiets
                .FirstOrDefault(x => x.DoiTuong == "Em bé")?.Gia ?? 0);


            decimal tongTien = adult * giaNL + child * giaTE + baby * giaEB;

            var vm = new NhapThongTinVM
            {
                IdTour = idTour,
                IdLich = idLich,
                TenTour = tour.TenTour ?? "",
                NgayKhoiHanh = lich.NgayKhoiHanh.ToDateTime(TimeOnly.MinValue),

                NguoiLon = adult,
                TreEm = child,
                EmBe = baby,

                GiaNguoiLon = giaNL,
                GiaTreEm = giaTE,
                GiaEmBe = giaEB,

                TongTien = tongTien
            };

            return View(vm);
        }

       [HttpPost]
        public IActionResult TaoDonVaChuyenSangThanhToan(NhapThongTinVM model)
        {
            int userId = HttpContext.Session.GetInt32("IdTaiKhoan") ?? 0;

            var don = new DonDatTour
            {
                IdTour = model.IdTour,
                IdLich = model.IdLich,
                IdTaiKhoan = userId,

                NguoiLon = model.NguoiLon,
                TreEm = model.TreEm,
                TreNho = model.EmBe,

                GhiChu = model.GhiChu,

                TongTien = 0, // ⭐ ĐỂ SQL TRIGGER TỰ TÍNH

                NgayDat = DateTime.Now,
                HanThanhToan = DateTime.Now.AddMinutes(10),
                TrangThai = "ChoThanhToan",
                DaThanhToan = false
            };

            _db.DonDatTours.Add(don);
            _db.SaveChanges();

            // ⭐ LOAD LẠI DỮ LIỆU SAU KHI TRIGGER CHẠY
            _db.Entry(don).Reload();

            return RedirectToAction("ThanhToan", new { idDon = don.IdDon });
        }
        
        public IActionResult ThanhToan(int idDon)
        {
            var don = _db.DonDatTours
                .Include(t => t.IdTourNavigation)
                .Include(t => t.IdLichNavigation)
                .FirstOrDefault(t => t.IdDon == idDon);

            if (don == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index", "Home");
            }

            if (don.TrangThai == "ChoThanhToan" && don.HanThanhToan < DateTime.Now)
            {
                don.TrangThai = "DaHuy";

                int soKhach = don.NguoiLon + don.TreEm + don.TreNho;

                don.IdLichNavigation.SoChoConLai += soKhach;
                don.IdTourNavigation.SoNguoiDaDat -= soKhach;

                _db.SaveChanges();

                TempData["Error"] = "⛔ Đơn đã hết hạn và bị hủy tự động!";
                return RedirectToAction("HoSo", "TaiKhoan");
            }

            return View(don);
        }

       
       public IActionResult XacNhanThanhToan(int idDon)
        {
            var don = _db.DonDatTours
                .Include(t => t.IdLichNavigation)
                .Include(t => t.IdTourNavigation)
                .FirstOrDefault(t => t.IdDon == idDon);

            if (don == null)
            {
                TempData["Error"] = "Đơn không tồn tại.";
                return RedirectToAction("Index", "Home");
            }

            // ===== HẾT HẠN → HỦY ĐƠN =====
            if (don.HanThanhToan < DateTime.Now)
            {
                don.TrangThai = "DaHuy";

                int soKhachHuy = don.NguoiLon + don.TreEm + don.TreNho;

                don.IdLichNavigation.SoChoConLai += soKhachHuy;
                don.IdTourNavigation.SoNguoiDaDat -= soKhachHuy;

                // 🔥 ÉP EF UPDATE
                _db.Entry(don).Property(x => x.DaThanhToan).IsModified = true;
                _db.Entry(don).Property(x => x.TrangThai).IsModified = true;
                _db.Entry(don).Property(x => x.TrangThai).IsModified = true;


                _db.Entry(don.IdLichNavigation).Property(x => x.SoChoConLai).IsModified = true;
                _db.Entry(don.IdTourNavigation).Property(x => x.SoNguoiDaDat).IsModified = true;

                _db.SaveChanges();
                TempData["Error"] = "Đơn đã hết hạn và bị hủy tự động!";
                return RedirectToAction("DonCuaToi", "TaiKhoan");
            }

            // ===== THANH TOÁN THÀNH CÔNG =====
            don.DaThanhToan = true;
            don.TrangThai = "ThanhToanThanhCong";
            don.TrangThaiThanhToan = "ThanhToanThanhCong";
            don.NgayThanhToan = DateTime.Now;

            int soKhach = don.NguoiLon + don.TreEm + don.TreNho;

            // Trừ chỗ và cộng lượt đặt
            don.IdLichNavigation.SoChoConLai -= soKhach;
            don.IdTourNavigation.SoNguoiDaDat += soKhach;

            // 🔥 ÉP UPDATE 2 bảng liên quan
            _db.Entry(don).Property(x => x.DaThanhToan).IsModified = true;
            _db.Entry(don).Property(x => x.TrangThai).IsModified = true;
            _db.Entry(don).Property(x => x.TrangThaiThanhToan).IsModified = true;
            _db.Entry(don).Property(x => x.NgayThanhToan).IsModified = true;

            _db.Entry(don.IdLichNavigation).Property(x => x.SoChoConLai).IsModified = true;
            _db.Entry(don.IdTourNavigation).Property(x => x.SoNguoiDaDat).IsModified = true;

            _db.SaveChanges();


            return RedirectToAction("HoanTat", new { idDon });
        }

     
        public IActionResult HoanTat(int idDon)
        {
            var don = _db.DonDatTours
                .Include(t => t.IdTourNavigation)
                .Include(t => t.IdLichNavigation)
                .FirstOrDefault(t => t.IdDon == idDon);

            if (don == null)
                return RedirectToAction("Index", "Home");

            return View(don);
        }
    }
}
