using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class SanphamDto
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự.")]
        public string ten_sanpham { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng chọn loại sản phẩm.")]
        public int id_loaisp { get; set; }

        [Required(ErrorMessage = "Thành tiền không được để trống.")]
        [Range(1, double.MaxValue, ErrorMessage = "Thành tiền phải lớn hơn 0.")]
        public decimal thanhtien { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm.")]
        public int soluong { get; set; } = 0;

        [Required(ErrorMessage = "Hình ảnh không được để trống.")]
        public IFormFile? hinhanh { get; set; }

        [Required(ErrorMessage = "Mã sản phẩm không được để trống.")]
        [MaxLength(50, ErrorMessage = "Mã sản phẩm không được vượt quá 50 ký tự.")]
        public string ma_sanpham { get; set; } = "";
    }
}
