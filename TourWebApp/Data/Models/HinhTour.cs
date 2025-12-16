using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("HinhTour")]
public partial class HinhTour
{
    [Key]
    public int IdHinh { get; set; }

    public int IdTour { get; set; }

    [StringLength(255)]
    public string UrlHinh { get; set; } = null!;

    [StringLength(255)]
    public string? MoTa { get; set; }

    public int? ThuTu { get; set; }

    [ForeignKey("IdTour")]
    [InverseProperty("HinhTours")]
    public virtual Tour IdTourNavigation { get; set; } = null!;
}
