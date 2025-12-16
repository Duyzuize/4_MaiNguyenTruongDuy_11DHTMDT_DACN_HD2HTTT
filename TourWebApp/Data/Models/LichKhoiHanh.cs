using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("LichKhoiHanh")]
[Index("IdTour", "NgayKhoiHanh", Name = "UX_Lich_TourDate", IsUnique = true)]
public partial class LichKhoiHanh
{
    [Key]
    public int IdLich { get; set; }

    public int IdTour { get; set; }

    public DateOnly NgayKhoiHanh { get; set; }

    [Precision(0)]
    public TimeOnly? GioKhoiHanh { get; set; }

    public int SoChoToiDa { get; set; }

    public int SoChoConLai { get; set; }

    [StringLength(20)]
    public string TrangThai { get; set; } = null!;

    [InverseProperty("IdLichNavigation")]
    public virtual ICollection<DonDatTour> DonDatTours { get; set; } = new List<DonDatTour>();

    [ForeignKey("IdTour")]
    [InverseProperty("LichKhoiHanhs")]
    public virtual Tour IdTourNavigation { get; set; } = null!;
}
