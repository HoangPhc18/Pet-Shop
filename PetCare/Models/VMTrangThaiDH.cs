namespace PetCare.Models
{
    public class VMTrangThaiDH
    {
        public int id_dh {  get; set; }
        public int id_nv { get; set; }
        public string trang_thai { get; set; } = "";
        public string? ghi_chu { get; set; }
        public string ma_donhang { get; set; } = "";
    }
}
