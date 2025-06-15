using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class Sanpham_loaiDto
    {
        [Required]
        [MaxLength(100)]
        public string name { get; set; }
        public IFormFile? hinh_anh { get; set; }
    }
}
