namespace PetCare.Models
{
    public class VMSanPham
    {
        public int id_sanpham {  get; set; }
        public string? ten_sanpham { get; set; }
        public int id_loaisp {  get; set; }
        public string? loai_sanpham { get; set; }
        public string? hinhanh { get; set; }
        public int soluong {  get; set; }
        public decimal thanhtien { get; set; }
        public string? ma_sanpham { get; set; }
    }
}
