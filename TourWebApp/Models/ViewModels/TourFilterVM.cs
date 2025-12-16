namespace TourWebApp.Models.ViewModels
{
    public class TourListVM
    {
        public List<Data.Models.Tour> Tours { get; set; } = new List<Data.Models.Tour>();

        // ========== BỘ LỌC ==========
        public string? DiaDiem { get; set; }
        public int? SoNgay { get; set; }
        public int? GiaMin { get; set; }
        public int? GiaMax { get; set; }
        public string? Sort { get; set; }
    }
}
