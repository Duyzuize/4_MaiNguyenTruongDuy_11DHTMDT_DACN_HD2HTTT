using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("LienHe")]
public partial class LienHe
{
    [Key]
    public int IdLienHe { get; set; }

    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string? DienThoai { get; set; }

    public string NoiDung { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime NgayGui { get; set; }
}
