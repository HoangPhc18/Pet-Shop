using Microsoft.AspNetCore.Mvc;
using petCare.Models;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers
{
    public class ThucungController : Controller
    {
        private readonly ApplicationDbContext context;

        public ThucungController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Create action (for adding new pets to a customer)
        public IActionResult Create(int id)
        {
            ViewData["IDKH"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult Create(ThucungDto thucungdto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IDKH"] = thucungdto.id_kh; // Preserve IDKH in case of errors
                return View(thucungdto);
            }

            var thucungs = new Thucung
            {
                ten_pet = thucungdto.ten_pet,
                ngaysinh_pet = thucungdto.ngaysinh_pet,
                giong_pet = thucungdto.giong_pet,
                cannang_pet = thucungdto.cannang_pet,
                id_kh = thucungdto.id_kh
            };

            context.Thucungs.Add(thucungs);
            context.SaveChanges();

            return RedirectToAction("Detail", "KhachHang", new { id = thucungs.id_kh });
        }

        // Edit action
        public IActionResult Edit(int id)
        {

            var pet = context.Thucungs.Find(id);
            if (pet == null)
            {
                return RedirectToAction("Detail", "KhachHang", new { id = pet.id_kh });
            }

            var thucungDto = new ThucungDto
            {
                ten_pet = pet.ten_pet,
                ngaysinh_pet = pet.ngaysinh_pet,
                giong_pet = pet.giong_pet,
                cannang_pet = pet.cannang_pet,
                id_kh = pet.id_kh
            };
            ViewData["IDKH"] = id;
            return View(thucungDto);
        }

        [HttpPost]
        public IActionResult Edit(ThucungDto thucungdto, int id)
        {
            var thucung = context.Thucungs.Find(id);
            if (id != thucung.id_pet)
            {
                return RedirectToAction("Detail", "Khachhang", new { id = thucungdto.id_kh });
            }

            if (!ModelState.IsValid)
            {
                ViewData["IDKH"] = thucungdto.id_kh; // Preserve IDKH in case of errors
                return View(thucungdto);
            }

                thucung.ten_pet = thucungdto.ten_pet;
                thucung.ngaysinh_pet = thucungdto.ngaysinh_pet;
                thucung.giong_pet = thucungdto.giong_pet;
                thucung.cannang_pet = thucungdto.cannang_pet;
                thucung.id_kh = thucungdto.id_kh;

            context.SaveChanges();

            return RedirectToAction("Detail", "KhachHang", new { id = thucung.id_kh });
        }

        [HttpPost]
        public IActionResult ThongBao(string message, string returnUrl)
        {
            ViewData["Message"] = message;
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }



        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pet = context.Thucungs.Find(id);
            if (pet == null)
            {
                return Json(new { success = false, message = "Thú cưng không tồn tại." });
            }

            try
            {
                context.Thucungs.Remove(pet);
                context.SaveChanges();
                return Json(new { success = true, message = "Xóa thú cưng thành công." });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xóa thú cưng." });
            }
        }

    }
}
