using X.PagedList;

namespace PetCare.Models
{
    public class VMHienThiShop
    {
        public List<Sanpham_Loai> loaisps { get; set; }
        public IEnumerable<Sanpham> sanphams { get; set; }
    }
}
