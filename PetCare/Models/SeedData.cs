using PetCare.Services;
using PetCare.Models;
using Microsoft.EntityFrameworkCore;

namespace PetCare.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Nhanviens.Any())
                {
                    return;
                }

                context.Nhanviens.AddRange(
                    new Nhanvien
                    {
                        ten_nv = "Dương Ngọc Phú",
                        sdt_nv = "0123456789",
                        email_nv = "duongngocphu2003@gmail.com",
                        chucvu_nv = "Admin",
                        matkhau_nv = "123"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
