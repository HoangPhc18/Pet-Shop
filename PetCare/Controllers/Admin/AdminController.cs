using Microsoft.AspNetCore.Mvc;
using PetCare.Models.Authentication;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext context;
        public AdminController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authentication]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("Username");
            if (userId == null)
            {
                HttpContext.Session.Clear();
            }
            var user = context.Nhanviens.Find(userId);

            ViewData["UserId"] = user?.id_nv;
            ViewData["UserName"] = user?.ten_nv;

            var doanhthu = context.Donhangs
                .Where(dh => dh.trang_thai == "Hoàn thành")
                .Sum(dh => (decimal?)dh.tong_tien) ?? 0;

            var donhang = context.Donhangs.Count();

            var lichhen = context.Lichhens.Count();

            var huyhen = context.Lichhens
                .Count(lh => lh.trang_thai == "Hủy Lịch Hẹn");

            var huydon = context.Donhangs
                .Count(lh => lh.trang_thai == "Hủy đơn");

            int tonghuy = huyhen + huydon;

            ViewData["DoanhThu"] = doanhthu;
            ViewData["DonHang"] = donhang;
            ViewData["LichHen"] = lichhen;
            if (doanhthu > 0)
            {
                ViewData["DoanhThu"] = doanhthu.ToString("N0");
            }
            else
            {
                ViewData["DoanhThu"] = 0;
            }

            if (donhang > 0)
            {
                ViewData["DonHang"] = donhang;
            }
            else
            {
                ViewData["DonHang"] = 0;
            }

            if (lichhen > 0)
            {
                ViewData["LichHen"] = lichhen;
            }
            else
            {
                ViewData["LichHen"] = 0;
            }

            if (tonghuy > 0)
            {
                ViewData["Huy"] = tonghuy;
            }
            else
            {
                ViewData["Huy"] = 0;
            }

            return View();
        }
    }
}
