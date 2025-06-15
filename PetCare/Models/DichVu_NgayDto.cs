using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class DichVu_NgayDto
    {
        public int id_dichvu_ngay { get; set; }
        public int id_dichvu { get; set; }
        [Required]
        public decimal gia_thanh { get; set; }
    }
}
