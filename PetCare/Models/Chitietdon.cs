using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCare.Models
{
    public class Chitietdon
    {
        [Key]
        public int id_chitietdh { get; set; }
        public int id_dh {  get; set; }
        public int id_sp { get; set; }
        public int soluong { get; set; }

        [ForeignKey("id_dh")]
        public Donhang Donhang { get; set; }
    }
}
