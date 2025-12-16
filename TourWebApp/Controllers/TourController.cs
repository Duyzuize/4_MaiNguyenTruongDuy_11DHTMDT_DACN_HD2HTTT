using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TourWebApp.Data.Models;

namespace TourWebApp.Controllers
{
    public class TourController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TourController(ApplicationDbContext db)
        {
            _db = db;
        }

        // TÁCH SỐ NGÀY TRONG CHUỖI "3N2Đ"
        private int ExtractNgay(string? thoiGian)
        {
            if (string.IsNullOrEmpty(thoiGian)) return 0;
            if (char.IsDigit(thoiGian[0]))
                return int.Parse(thoiGian[0].ToString());
            return 0;
        }

        // LỊCH GẦN NHẤT
        private LichKhoiHanh? GetNextLich(int idTour)
        {
            return _db.LichKhoiHanhs
                .Where(l => l.IdTour == idTour &&
                            l.NgayKhoiHanh >= DateOnly.FromDateTime(DateTime.Now))
                .OrderBy(l => l.NgayKhoiHanh)
                .ThenBy(l => l.GioKhoiHanh)
                .FirstOrDefault();
        }

       public IActionResult TatCa(
        string? diadiem,
        int? songay,
        decimal? giamin,
        decimal? giamax,
        string? sort,
        int page = 1)
        {
            int pageSize = 4;

            var query = _db.Tours
                .Where(t => t.TrangThai == true)
                .AsQueryable();

            // === SEARCH ĐỊA ĐIỂM ===
           if (!string.IsNullOrWhiteSpace(diadiem))
{
                string keyword = diadiem.Trim().ToLower();

                query = query.Where(t =>
                    t.DiaDiem != null &&
                    t.DiaDiem.ToLower().Contains(keyword)
                );
            }

            // === FILTER SỐ NGÀY ===
            if (songay.HasValue && songay > 0)
            {
                query = query.AsEnumerable()
                            .Where(t => ExtractNgay(t.ThoiGian) == songay)
                            .AsQueryable();
            }

            // === FILTER GIÁ ===
            if (giamin.HasValue) query = query.Where(t => t.GiaKhuyenMai >= giamin);
            if (giamax.HasValue) query = query.Where(t => t.GiaKhuyenMai <= giamax);

            // === SORT ===
            switch (sort)
            {
                case "gia_tang": query = query.OrderBy(t => t.GiaKhuyenMai); break;
                case "gia_giam": query = query.OrderByDescending(t => t.GiaKhuyenMai); break;
                case "ten": query = query.OrderBy(t => t.TenTour); break;
                default: query = query.OrderByDescending(t => t.LuotXem); break;
            }

            // === PHÂN TRANG ===
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var tours = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            ViewBag.LichGanNhat = tours.ToDictionary(
                t => t.IdTour,
                t => GetNextLich(t.IdTour)
            );

            return View(tours);
        }


        // TOUR THEO LOẠI
        public IActionResult TheoLoai(int id)
        {
            var loai = _db.LoaiTours.FirstOrDefault(l => l.IdLoaiTour == id);
            ViewBag.TenLoai = loai?.TenLoai ?? "Tour";

            var tours = _db.Tours
                .Where(t => t.TrangThai == true && t.IdLoaiTour == id)
                .OrderByDescending(t => t.LuotXem)
                .ToList();

            ViewBag.LichGanNhat = tours.ToDictionary(
                t => t.IdTour,
                t => GetNextLich(t.IdTour)
            );

            return View(tours);
        }

        [HttpGet]
        public IActionResult GoiyDiaDiem(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return Json(new List<string>());

            keyword = keyword.ToLower();

            var results = _db.Tours
                .Where(t => t.DiaDiem != null && t.DiaDiem.ToLower().Contains(keyword))
                .Select(t => t.DiaDiem)
                .Distinct()
                .Take(10)
                .ToList();

            return Json(results);
        }

        // CHI TIẾT TOUR
        public IActionResult ChiTiet(int id)
        {
            var tour = _db.Tours
                .Include(t => t.HinhTours)
                .Include(t => t.LichKhoiHanhs)
                .Include(t => t.TourGiaChiTiets)
                .FirstOrDefault(t => t.IdTour == id);

            if (tour == null)
                return NotFound();

            // +1 lượt xem
            tour.LuotXem++;
            _db.SaveChanges();

            // LỊCH GẦN NHẤT
            ViewBag.LichGanNhat = _db.LichKhoiHanhs
                .Where(l => l.IdTour == id)
                .OrderBy(l => l.NgayKhoiHanh)
                .ThenBy(l => l.GioKhoiHanh)
                .FirstOrDefault();

            // GIÁ TỪ CS CSDL – KHÔNG LẤY PHỤ THU
            ViewBag.GiaNguoiLon = tour.TourGiaChiTiets.FirstOrDefault(x => x.DoiTuong == "Người lớn");
            ViewBag.GiaTreEm    = tour.TourGiaChiTiets.FirstOrDefault(x => x.DoiTuong == "Trẻ em");
            ViewBag.GiaEmBe     = tour.TourGiaChiTiets.FirstOrDefault(x => x.DoiTuong == "Em bé");

            // TOUR LIÊN QUAN
            ViewBag.TourLienQuan = _db.Tours
                .Where(t => t.IdLoaiTour == tour.IdLoaiTour && t.IdTour != id)
                .OrderByDescending(t => t.LuotXem)
                .Take(3)
                .ToList();

            return View(tour);
        }
        public IActionResult Index()
        {
            return RedirectToAction("TatCa");
        }
    }
}
