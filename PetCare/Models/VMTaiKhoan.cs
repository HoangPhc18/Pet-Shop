namespace PetCare.Models
{
    public class VMTaiKhoan
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Khachhang Khachhang { get; set; }
        public Nhanvien Nhanvien { get; set; }
    }
}
