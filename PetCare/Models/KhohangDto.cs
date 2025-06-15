using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class KhohangDto
    {
        public int id_sp { get; set; }
        [Required]
        public string ma_sanpham { get; set; }
        [Required]
        public int soluong { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
