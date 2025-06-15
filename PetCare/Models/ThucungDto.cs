using System;
using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class ThucungDto
    {
        [Required]
        [MaxLength(100)]
        public string ten_pet { get; set; } = "";

        [Required]
        public DateTime ngaysinh_pet { get; set; }

        [MaxLength(100)]
        public string giong_pet { get; set; } = "";

        public float cannang_pet { get; set; }

        public int id_kh { get; set; }
    }
}
