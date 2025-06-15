using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class DichVu_Ngay
    {
        [Key]
        public int id_dichvu_ngay { get; set; }
        public int id_dichvu { get; set; }
        [Precision(16, 2)]
        public decimal gia_thanh { get; set; }
    }
}
