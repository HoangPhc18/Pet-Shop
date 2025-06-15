using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class NhanVienDto
    {
        public int id_nv { get; set; }

        [Required]
        [MaxLength(100)]
        public string ten_nv { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string sdt_nv { get; set; } = "";

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string email_nv { get; set; } = "";

        [Required]
        [MaxLength(100)]
        public string chucvu_nv { get; set; } = "";

        [DataType(DataType.Password)]
        public string? matkhau_nv { get; set; }

        [DataType(DataType.Password)]
        [Compare("matkhau_nv", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmPassword { get; set; }
    }
}
