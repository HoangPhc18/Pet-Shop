using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class DichVu_CanNang
    {
        [Key]
        public int id_dichvu_can_nang { get; set; }
        public int id_dichvu { get; set; }
        public float min_can_nang {  get; set; }
        public float max_can_nang { get; set; }
        [Precision(16, 2)]
        public decimal gia_thanh { get; set; }
    }
}
