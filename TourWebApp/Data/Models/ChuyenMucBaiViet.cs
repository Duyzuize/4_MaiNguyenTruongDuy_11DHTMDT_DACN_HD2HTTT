using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

[Table("ChuyenMucBaiViet")]
public partial class ChuyenMucBaiViet
{
    [Key]
    public int IdCM { get; set; }

    [StringLength(100)]
    public string TenCM { get; set; } = null!;

     [StringLength(255)]
    public string? MoTa { get; set; }

     public bool TrangThai { get; set; } = true;

    [InverseProperty("IdCMNavigation")]
    public virtual ICollection<BaiViet> BaiViets { get; set; } = new List<BaiViet>();
}
