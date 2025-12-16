using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("BinhLuanTour")]
public partial class BinhLuanTour
{
    [Key]
    public int IdBL { get; set; }

    public int? IdTour { get; set; }
    public int? IdBaiViet { get; set; }
    public int? IdTaiKhoan { get; set; }

    [StringLength(100)]
    public string Ten { get; set; } = null!;

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? DienThoai { get; set; }

    public string NoiDung { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayBL { get; set; }

    public string? PhanHoiAdm { get; set; }

    public bool HienThi { get; set; }

    // --- Navigation đúng chuẩn SQL & DbContext ---
    [ForeignKey("IdTour")]
    [InverseProperty("BinhLuanTours")]
    public virtual Tour? IdTourNavigation { get; set; }

    [ForeignKey("IdBaiViet")]
    [InverseProperty("BinhLuanTours")]
    public virtual BaiViet? IdBaiVietNavigation { get; set; }
  
    [ForeignKey("IdTaiKhoan")]
    public virtual TaiKhoan? IdTaiKhoanNavigation { get; set; }
}
