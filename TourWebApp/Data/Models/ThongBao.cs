using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("ThongBao")]
public partial class ThongBao
{
    [Key]
    public int IdThongBao { get; set; }

    public int? IdDon { get; set; }

    public string NoiDung { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayTao { get; set; }

    public bool DaDoc { get; set; }

    public int IdNguoiNhan { get; set; }

    [StringLength(200)]
    public string TieuDe { get; set; } = null!;

    [StringLength(255)]
    public string? LienKet { get; set; }

    [ForeignKey("IdDon")]
    [InverseProperty("ThongBaos")]
    public virtual DonDatTour? IdDonNavigation { get; set; }
}
