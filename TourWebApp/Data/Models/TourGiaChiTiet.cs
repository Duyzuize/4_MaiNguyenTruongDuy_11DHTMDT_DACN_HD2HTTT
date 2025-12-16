using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("TourGiaChiTiet")]
[Index("IdTour", "DoiTuong", Name = "UX_Gia_Tour_DoiTuong", IsUnique = true)]
public partial class TourGiaChiTiet
{
    [Key]
    public int IdGia { get; set; }

    public int IdTour { get; set; }

    [StringLength(20)]
    public string DoiTuong { get; set; } = null!;

    [Column(TypeName = "decimal(18, 0)")]
    public decimal Gia { get; set; }

    [StringLength(200)]
    public string? GhiChu { get; set; }

    [ForeignKey("IdTour")]
    [InverseProperty("TourGiaChiTiets")]
    public virtual Tour IdTourNavigation { get; set; } = null!;
}
