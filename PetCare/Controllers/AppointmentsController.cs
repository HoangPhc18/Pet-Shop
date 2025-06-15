using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using petCare.Models;
using PetCare.Models;
using PetCare.Models.Authentication;
using PetCare.Services;

namespace PetCare.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public AppointmentsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        [HttpGet]
        [Authentication]
        [Route("/Information", Name = "Information")]
        public IActionResult Information(int? id_pet)
        {
            var user = HttpContext.Session.GetInt32("Username");
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var khachhang = context.Khachhangs.Find(user);
            if (khachhang == null)
            {
                return NotFound("Khách hàng không tồn tại.");
            }

            var thongtin = new VMChitietLichHen
            {
                id_kh = khachhang.id_kh,
                ten_kh = khachhang.ten_kh,
                sdt_kh = khachhang.sdt_kh
            };
            id_pet = id_pet ?? 0;

            var thucung = context.Thucungs.Where(tc => tc.id_kh == khachhang.id_kh).ToList();

            if (id_pet != null)
            {
                var selectedPet = thucung.FirstOrDefault(tc => tc.id_pet == id_pet);
                if (selectedPet != null)
                {
                    thongtin.ten_tc = selectedPet.ten_pet;
                    thongtin.giong_tc = selectedPet.giong_pet;
                    thongtin.can_nang = selectedPet.cannang_pet;
                    thongtin.id_tc = selectedPet.id_pet;
                }
            }
            thucung.Insert(0, new Thucung { id_pet = 0, ten_pet = "--- Chọn Pet ---" });
            ViewBag.ThuCungs = new SelectList(thucung, "id_pet", "ten_pet", id_pet);
            ViewBag.DichVuOption = context.DichVus.ToList();

            ViewData["PetId"] = id_pet;
            ViewData["UserId"] = khachhang.id_kh;
            ViewData["UserName"] = khachhang.ten_kh;

            return View(thongtin);
        }
        public IActionResult Success()
        {
            return View();
        }


        [HttpPost]
        [Route("/Information", Name = "Information")]
        public IActionResult Information(int id_tc, VMChitietLichHen lichhenDto)
        {
            // Load danh sách thú cưng vào ViewBag
            ViewBag.ThuCungs = new SelectList(
                context.Thucungs.Where(tc => tc.id_kh == lichhenDto.id_kh).ToList(),
                "id_pet",
                "ten_pet",
                lichhenDto.id_tc);

            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, trả về form để sửa lại
                return View(lichhenDto);
            }

            // Tạo đối tượng lịch hẹn
            var lichhen = new Lichhen
            {
                id_kh = lichhenDto.id_kh,
                id_tc = id_tc,
                id_nv = null,
                ngay_hen = lichhenDto.ngay_hen,
                id_dichvu = lichhenDto.id_dichvu,
                ghi_chu = lichhenDto.ghi_chu,
                CreateAt = DateTime.Now,
                trang_thai = "Đang chờ xác nhận"
            };

            // Lưu vào cơ sở dữ liệu
            context.Lichhens.Add(lichhen);
            context.SaveChanges();

            // Chuyển hướng đến trang thông báo thành công
            return RedirectToAction("Success", "Appointments");
        }




    }
}
