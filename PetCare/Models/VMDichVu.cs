namespace PetCare.Models
{
    public class VMDichVu
    {
        public DichVu DichVu { get; set; }
        public List<DichVu_CanNang> DichVu_CanNangs { get; set; }
        public List<DichVu_Ngay> DichVu_Ngays { get; set; }
    }
}
