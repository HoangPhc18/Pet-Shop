using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class Khohang
    {
        [Key]
        public int id_kho { get; set; }
        public int id_sp { get; set; }
        public int soluong {  get; set; }
        public DateTime CreatedAt { get; set; }
        // Navigation property
        public Sanpham Sanpham { get; set; }
    }
}
