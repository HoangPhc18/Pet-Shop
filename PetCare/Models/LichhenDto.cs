using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class LichhenDto
    {
        public int id_nv { get; set; }
        [Required]
        public int id_kh { get; set; }
        [Required]
        public int id_tc { get; set; }
        public DateTime ngay_hen { get; set; }
        [Required]
        public int DichVu { get; set; } //Id Dịch Vụ

        public string? ghi_chu { get; set; }
        public decimal tong_tien { get; set; }
        public DateTime CreateAt { get; set; }
        [MaxLength(100)]
        public string trang_thai { get; set; } = "";
    }
}
