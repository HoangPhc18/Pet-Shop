using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class Sanpham_Loai
    {
        [Key]
        public int id { get; set; }
        [MaxLength(100)]
        public string name { get; set; }
        [MaxLength(100)]
        public string hinh_anh { get; set; }
    }
}
