using Microsoft.EntityFrameworkCore;
using petCare.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Models
{
    public class Lichhen
    {
        [Key]
        public int id_lichhen { get; set; }
        public int? id_nv {  get; set; }
        public int id_kh { get; set; }
        public int id_tc { get; set; }
        public int id_dichvu { get; set; } // Foreign key for DichVu

        public DateTime ngay_hen { get; set; }

        public string? ghi_chu { get; set; }

        [MaxLength(100)]
        public string trang_thai { get; set; } = "";

        public DateTime CreateAt { get; set; }

        [ForeignKey("id_kh")]
        public Khachhang Khachhang { get; set; }

        [ForeignKey("id_tc")]
        public Thucung Thucung { get; set; }

        [ForeignKey("id_dichvu")]
        public DichVu DichVu { get; set; }
    }
}
