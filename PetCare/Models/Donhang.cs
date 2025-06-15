using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Models
{
    public class Donhang
    {
        [Key]
        public int id_dh { get; set; }
        [MaxLength(100)]
        public string ma_dh { get; set; } = "";
        public int id_nv { get; set; }
        public int id_kh { get; set; }
        public string diachi_giao { get; set; } = "";
        public string? ghi_chu { get; set; }
        [Precision(16, 2)]
        public decimal tong_tien { get; set; }
        [MaxLength(100)]
        public string trang_thai { get; set; } = "";
        public DateTime CreateAt { get; set; }

        // Navigation property to link to customer
        [ForeignKey("id_kh")]
        public Khachhang? Khachhang { get; set; }

        // Navigation property for order details
        public virtual ICollection<Chitietdon> Chitietdons { get; set; }
    }
}
