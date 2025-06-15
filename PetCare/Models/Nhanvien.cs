using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Models
{
    public class Nhanvien
    {
        [Key]
        public int id_nv { get; set; }
        [MaxLength(100)]
        public string ten_nv { get; set; } = "";
        [MaxLength(100)]
        public string sdt_nv {  get; set; } = "";
        [MaxLength(100)]
        public string email_nv { get; set; } = "";
        [MaxLength(100)]
        public string chucvu_nv { get; set; } = "";
        public string matkhau_nv { get; set; } = "";
    }
}
