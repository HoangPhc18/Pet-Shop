using petCare.Models;
using System.Collections.Generic;

namespace PetCare.Models
{
    public class VMKhachHangThuCung
    {
        public Khachhang Khachhang { get; set; }
        public List<Thucung> Thucungs { get; set; }
    }
}
