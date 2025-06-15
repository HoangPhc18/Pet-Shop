namespace PetCare.Models
{
    public class VMChitietdh
    {
        public int id_dh {  get; set; }
        public string ten_nv { get; set; } = "";
        public string ten_kh { get; set; } = "";
        public string sdt_kh { get; set; } = "";
        public string diachi_giao { get; set; } = "";
        public string trang_thai { get; set; } = "";
        public decimal tong_tien { get; set; }
        public DateTime ngay_tao { get; set; }
        public string ma_dh {  get; set; }
        public string ghi_chu { get; set; }
        public List<ChiTietDonHang> giohang { get; set; } = new List<ChiTietDonHang>();
    }

    public class ChiTietDonHang
    {
        public int id_sp { get; set; }
        public string ten_sp { get; set; } = "";
        public int so_luong {  get; set; }
        public decimal gia_sp { get; set; }
        public decimal thanh_tien { get; set; }
        public string hinh_anh { get; set; } = "";
    }
}
