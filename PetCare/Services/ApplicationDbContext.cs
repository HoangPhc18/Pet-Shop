using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using petCare.Models;
using PetCare.Models;

namespace PetCare.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Sanpham_Loai> Sanpham_loais { get; set; }
        public DbSet<Sanpham> Sanphams { get; set; }
        public DbSet<DichVu_CanNang> DichVu_CanNangs { get; set; }
        public DbSet<DichVu_Ngay> DichVu_Ngays { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<Thucung> Thucungs { get; set; }
        public DbSet<Khachhang> Khachhangs { get; set; }
        public DbSet<Nhanvien> Nhanviens { get; set; }
        public DbSet<Lichhen> Lichhens { get; set; }
        public DbSet<Chitietlich> Chitietlichs { get; set; }
        public DbSet<Donhang> Donhangs { get; set; }
        public DbSet<Chitietdon> Chitietdons { get; set; }
        public DbSet<Khohang> Khohangs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Khohang>()
                .HasOne(k => k.Sanpham)
                .WithMany()
                .HasForeignKey(k => k.id_sp);

            modelBuilder.Entity<Sanpham>()
                .HasOne(sp => sp.sanpham_loai)
                .WithMany()
                .HasForeignKey(sp => sp.id_loaisp);

            // Khachhang relationships
            modelBuilder.Entity<Khachhang>()
                .HasMany(kh => kh.Thucungs)
                .WithOne(tc => tc.Khachhang)
                .HasForeignKey(tc => tc.id_kh);

            modelBuilder.Entity<Khachhang>()
                .HasMany(kh => kh.Lichhens)
                .WithOne(lh => lh.Khachhang)
                .HasForeignKey(lh => lh.id_kh);

            // Disable cascading delete for Lichhen -> Thucung relationship
            modelBuilder.Entity<Lichhen>()
                .HasOne(lh => lh.Thucung)
                .WithMany(tc => tc.Lichhens)
                .HasForeignKey(lh => lh.id_tc)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure other relationships as needed
            modelBuilder.Entity<Lichhen>()
                .HasOne(lh => lh.Khachhang)
                .WithMany(kh => kh.Lichhens)
                .HasForeignKey(lh => lh.id_kh)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lichhen>()
                .HasOne(lh => lh.DichVu)
                .WithMany()
                .HasForeignKey(lh => lh.id_dichvu)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Donhang>()
                .HasMany(d => d.Chitietdons)
                .WithOne(c => c.Donhang)
                .HasForeignKey(c => c.id_dh)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete

            modelBuilder.Entity<Donhang>()
                .HasOne(d => d.Khachhang)
                .WithMany(kh => kh.Donhangs)
                .HasForeignKey(d => d.id_kh)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
