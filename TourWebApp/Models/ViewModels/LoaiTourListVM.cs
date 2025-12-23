namespace TourWebApp.Models.ViewModels
{
    public class LoaiTourListVM
    {
        public int IdLoaiTour { get; set; }
        public string TenLoai { get; set; } = "";
        public string? MoTa { get; set; }
        public int SoTour { get; set; }
    }
}
