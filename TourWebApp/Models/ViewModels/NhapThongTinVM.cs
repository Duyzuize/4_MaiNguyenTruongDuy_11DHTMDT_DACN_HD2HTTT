namespace TourWebApp.Models.ViewModels
{
    public class NhapThongTinVM
    {
        public int IdTour { get; set; }
        public int IdLich { get; set; }

        public string TenTour { get; set; } = string.Empty;
        public DateTime NgayKhoiHanh { get; set; }

        // Số lượng khách
        public int NguoiLon { get; set; }
        public int TreEm { get; set; }
        public int EmBe { get; set; }

        // Giá (chỉ để HIỂN THỊ trên form, không tin client)
        public decimal GiaNguoiLon { get; set; }
        public decimal GiaTreEm { get; set; }
        public decimal GiaEmBe { get; set; }

        // Tổng tiền để hiển thị trên form
        public decimal TongTien { get; set; }

        public int TongKhach => NguoiLon + TreEm + EmBe;

        // Thông tin liên hệ
        public string HoTen { get; set; } = string.Empty;
        public string SDT  { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string GhiChu { get; set; } = string.Empty;
    }
}
