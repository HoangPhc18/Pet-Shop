using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class VMThongtinthanhtoan
    {
        public string ten_kh { get; set; } = "";
        public string sdt_kh { get; set; } = "";
        [MaxLength(100)]
        public string diachi_giao { get; set; } = "";
        public string? ghi_chu { get; set; }
        public List<Giohang> giohangs { get; set; }
    }
}
