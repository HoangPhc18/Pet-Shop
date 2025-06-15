using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class DichVu_CanNangDto
    {
        public int id_dichvu_can_nang { get; set; }
        public int id_dichvu { get; set; }
        [Required]
        public float min_can_nang { get; set; }
        [Required]
        public float max_can_nang { get; set; }
        [Required]
        [Precision(16, 2)]
        public decimal gia_thanh { get; set; }
    }
}
