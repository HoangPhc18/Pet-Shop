using petCare.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Models
{
    public class Khachhang
    {
        [Key]
        public int id_kh { get; set; }

        [MaxLength(100)]
        public string ten_kh { get; set; } = "";

        [MaxLength(100)]
        public string sdt_kh { get; set; } = "";

        [MaxLength(100)]
        public string diachi_kh { get; set; } = "";

        [MaxLength(100)]
        public string matkhau { get; set; } = "";

        public ICollection<Thucung> Thucungs { get; set; } = new List<Thucung>();

        public ICollection<Lichhen> Lichhens { get; set; } = new List<Lichhen>();
        public ICollection<Donhang>? Donhangs { get; set; }
    }
}
