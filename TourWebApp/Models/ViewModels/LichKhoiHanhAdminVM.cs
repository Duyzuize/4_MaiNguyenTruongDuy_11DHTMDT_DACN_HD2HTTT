namespace TourWebApp.ViewModels
{
    public class LichKhoiHanhAdminVM
    {
        public int IdLich { get; set; }
        public int IdTour { get; set; }

        // 👉 Thêm default để khỏi warning non-null
        public string TenTour { get; set; } = string.Empty;

        public DateOnly NgayKhoiHanh { get; set; }
        public TimeOnly? GioKhoiHanh { get; set; }

        public int SoChoToiDa { get; set; }
        public int SoChoConLai { get; set; }

        public string TrangThai { get; set; } = string.Empty;

        // 👉 CHỈ 1 property SoDonDangKy thôi
        public int SoDonDangKy { get; set; }
    }
}
