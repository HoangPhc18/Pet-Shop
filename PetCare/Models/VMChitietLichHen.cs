using petCare.Models;

namespace PetCare.Models
{
    public class VMChitietLichHen
    {
        public int id_lich {  get; set; }
        public int id_kh { get; set; }
        public string ten_kh { get; set; } = "";
        public string sdt_kh { get; set; } = "";

        public int id_tc { get; set; }
        public string ten_tc { get; set; } = "";
        public string giong_tc { get; set; }
        public float can_nang { get; set; }

        public DateTime ngay_hen { get; set; }
        public int? id_nv {  get; set; }
        public string? ten_nv { get; set; }
        public int id_dichvu { get; set; } // ID of the selected service
        public string ten_dichvu { get; set; } = ""; // Name of the selected service
        public string? ghi_chu { get; set; }
        public string trang_thai { get; set; } = "";
    }
}
