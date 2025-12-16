using System.Collections.Generic;
using TourWebApp.Data.Models;

namespace TourWebApp.ViewModels
{
    public class QuanLyTourViewModel
    {
        public List<Tour> Tours { get; set; } = new();
        public List<LoaiTour> LoaiTours { get; set; } = new();

        // lọc
        public int? IdLoaiTour { get; set; }
        public string? TuKhoa { get; set; }
    }
}
