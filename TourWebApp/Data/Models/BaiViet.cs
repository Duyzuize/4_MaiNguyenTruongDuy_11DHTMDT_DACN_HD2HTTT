using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("BaiViet")]
public partial class BaiViet
{
    [Key]
    public int IdBaiViet { get; set; }

    public int IdCM { get; set; }

    public int IdTaiKhoan { get; set; } 

    [StringLength(255)]
    public string TieuDe { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    [StringLength(255)]
    public string? HinhAnh { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime NgayDang { get; set; }

    public bool TrangThai { get; set; }

    [StringLength(100)]
    public string? TacGia { get; set; }

    [ForeignKey("IdTaiKhoan")]
    public virtual TaiKhoan? IdTaiKhoanNavigation { get; set; }

    [ForeignKey("IdCM")]
    [InverseProperty("BaiViets")]
    public virtual ChuyenMucBaiViet IdCMNavigation { get; set; } = null!;

    [InverseProperty("IdBaiVietNavigation")]
    public virtual ICollection<BinhLuanTour> BinhLuanTours { get; set; } = new List<BinhLuanTour>();
}
