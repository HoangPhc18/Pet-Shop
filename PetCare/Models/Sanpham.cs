using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class Sanpham
    {
        [Key]
        public int id_sanpham { get; set; }
        [MaxLength(100)]
        public string ten_sanpham { get; set; } = "";
        public  int id_loaisp { get; set; }
        [Precision(16, 2)]
        public decimal thanhtien { get; set; }
        public int soluong { get; set; }
        [MaxLength(100)]
        public string hinhanh { get; set; } = "";
        public string ma_sanpham { get; set; } = "";
        public Sanpham_Loai sanpham_loai { get; set; }
    }
}
