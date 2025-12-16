using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TourWebApp.Data.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaiViet> BaiViets { get; set; }

    public virtual DbSet<BinhLuanTour> BinhLuanTours { get; set; }

    public virtual DbSet<ChuyenMucBaiViet> ChuyenMucBaiViets { get; set; }

    public virtual DbSet<DonDatTour> DonDatTours { get; set; }

    public virtual DbSet<HinhTour> HinhTours { get; set; }

    public virtual DbSet<LichKhoiHanh> LichKhoiHanhs { get; set; }

    public virtual DbSet<LienHe> LienHes { get; set; }

    public virtual DbSet<LoaiTour> LoaiTours { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourGiaChiTiet> TourGiaChiTiets { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaiViet>(entity =>
        {
            entity.HasKey(e => e.IdBaiViet).HasName("PK__BaiViet__42161C7A768D7ED0");

            entity.Property(e => e.NgayDang).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.IdCMNavigation).WithMany(p => p.BaiViets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BV_CM");
        });

        modelBuilder.Entity<BinhLuanTour>(entity =>
        {
            entity.HasKey(e => e.IdBL);

            entity.Property(e => e.HienThi).HasDefaultValue(true);
            entity.Property(e => e.NgayBL).HasDefaultValueSql("(getdate())");

            // ✅ Bình luận - Tour
            entity.HasOne(d => d.IdTourNavigation)
                .WithMany(p => p.BinhLuanTours)
                .HasForeignKey(d => d.IdTour)
                .HasConstraintName("FK_BL_Tour");

            // ✅ Bình luận - Bài viết (QUAN TRỌNG NHẤT)
            entity.HasOne(d => d.IdBaiVietNavigation)
                .WithMany(p => p.BinhLuanTours) // ✅ PHẢI TRÙNG Model BaiViet
                .HasForeignKey(d => d.IdBaiViet)
                .HasConstraintName("FK_BL_BaiViet");

            // ✅ Bình luận - Tài khoản
            entity.HasOne(d => d.IdTaiKhoanNavigation)
                .WithMany()
                .HasForeignKey(d => d.IdTaiKhoan)
                .HasConstraintName("FK_BL_TaiKhoan");
        });


        modelBuilder.Entity<ChuyenMucBaiViet>(entity =>
        {
            entity.HasKey(e => e.IdCM).HasName("PK__ChuyenMu__B773908EED1AE3C1");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<DonDatTour>(entity =>
        {
            entity.HasKey(e => e.IdDon).HasName("PK__DonDatTo__0E65F8ECE406AC64");

            entity.ToTable("DonDatTour", tb =>
                {
                    tb.HasTrigger("Trg_DonDat_AfterInsert");
                    tb.HasTrigger("Trg_DonDat_AfterUpdate");
                    tb.HasTrigger("Trg_DonDat_Overbook");
                });

            entity.Property(e => e.MaBooking).HasDefaultValueSql("(('BPT'+CONVERT([char](6),getdate(),(12)))+right('0000'+CONVERT([varchar](4),NEXT VALUE FOR [dbo].[SeqBooking]),(4)))");
            entity.Property(e => e.NgayDat).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue("Đã đặt");

            entity.HasOne(d => d.IdLichNavigation).WithMany(p => p.DonDatTours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Don_Lich");

            entity.HasOne(d => d.IdTaiKhoanNavigation).WithMany(p => p.DonDatTours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Don_TK");

            entity.HasOne(d => d.IdTourNavigation).WithMany(p => p.DonDatTours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Don_Tour");
        });

        modelBuilder.Entity<HinhTour>(entity =>
        {
            entity.HasKey(e => e.IdHinh).HasName("PK__HinhTour__50E95053ED5032E9");

            entity.HasOne(d => d.IdTourNavigation).WithMany(p => p.HinhTours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Hinh_Tour");
        });

        modelBuilder.Entity<LichKhoiHanh>(entity =>
        {
            entity.HasKey(e => e.IdLich).HasName("PK__LichKhoi__31D90F9E140D933B");

            // ✅ KHAI BÁO TẤT CẢ TRIGGER CỦA BẢNG
            entity.ToTable("LichKhoiHanh", tb =>
            {
                tb.HasTrigger("TRG_KhongChoSuaLichDaCoDon");
                tb.HasTrigger("TRG_KhongChoXoaLichDaCoDon");
                tb.HasTrigger("TRG_UpdateTrangThaiLich");
            });

            entity.Property(e => e.TrangThai)
                .HasDefaultValue("Đang mở");

            entity.HasOne(d => d.IdTourNavigation)
                .WithMany(p => p.LichKhoiHanhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lich_Tour");
        });

        modelBuilder.Entity<LienHe>(entity =>
        {
            entity.HasKey(e => e.IdLienHe).HasName("PK__LienHe__03AC912A9937FC66");

            entity.Property(e => e.NgayGui).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<LoaiTour>(entity =>
        {
            entity.HasKey(e => e.IdLoaiTour).HasName("PK__LoaiTour__E5093BFE7C724882");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.IdTaiKhoan).HasName("PK__TaiKhoan__9A53D3DD7FFA2B72");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.IdThongBao).HasName("PK__ThongBao__43FDD6C82DED7477");

            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TieuDe).HasDefaultValue("");

            entity.HasOne(d => d.IdDonNavigation).WithMany(p => p.ThongBaos).HasConstraintName("FK_TB_Don");
        });

        modelBuilder.Entity<Tour>(entity =>
        {
            entity.HasKey(e => e.IdTour).HasName("PK__Tour__860C736F4B0DB621");

            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.IdLoaiTourNavigation).WithMany(p => p.Tours)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tour_LoaiTour");
        });

        modelBuilder.Entity<TourGiaChiTiet>(entity =>
        {
            entity.HasKey(e => e.IdGia).HasName("PK__TourGiaC__0C9FA30AF4F8A895");

            entity.HasOne(d => d.IdTourNavigation).WithMany(p => p.TourGiaChiTiets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gia_Tour");
        });
        modelBuilder.HasSequence<int>("SeqBooking");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
