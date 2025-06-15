namespace PetCare.Models
{
    public class VMKhoSanPham
    {
        public int id_kho { get; set; }
        public int id_sp { get; set; }
        public string ten_sanpham { get; set; }
        public int soluong { get; set; }
        public DateTime ngaynhap { get; set; }
    }
}
