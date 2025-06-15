using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class KhachhangDto
    {
        public int id_kh { get; set; }

        [Required]
        [MaxLength(100)]
        public string ten_kh { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string sdt_kh { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string diachi_kh { get; set; } = "";

        [DataType(DataType.Password)]
        public string? matkhau_kh { get; set; }

        [DataType(DataType.Password)]
        [Compare("matkhau_kh", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmPassword { get; set; }

    }
}
