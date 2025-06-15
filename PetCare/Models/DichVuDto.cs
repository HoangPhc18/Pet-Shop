using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class DichVuDto
    {
        [Required]
        public int id_dichvu { get; set; }
        [Required]
        [MaxLength(100)]
        public string? ten_dichvu { get; set; }
        [Required]
        public int loai_dichvu { get; set; }
    }
}
