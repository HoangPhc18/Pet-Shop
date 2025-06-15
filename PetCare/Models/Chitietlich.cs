using System.ComponentModel.DataAnnotations;

namespace PetCare.Models
{
    public class Chitietlich
    {
        [Key]
        public int id_ctlich {  get; set; }
        public int id_dichvu { get; set; }
        public string ghichu { get; set; } = "";
        public int id_lich { get; set; }
    }
}
