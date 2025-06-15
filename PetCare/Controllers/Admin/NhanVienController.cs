using Microsoft.AspNetCore.Mvc;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public NhanVienController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var nhanvien = context.Nhanviens.OrderByDescending(nv => nv.id_nv).ToList();
            return View(nhanvien);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NhanVienDto nhanVienDto)
        {
            if (!ModelState.IsValid)
            {
                return View(nhanVienDto);
            }

            // Create a new NhanVien entity
            Nhanvien nhanvien = new Nhanvien
            {
                ten_nv = nhanVienDto.ten_nv,
                sdt_nv = nhanVienDto.sdt_nv,
                email_nv = nhanVienDto.email_nv,
                chucvu_nv = nhanVienDto.chucvu_nv,
                matkhau_nv = nhanVienDto.matkhau_nv,
            };

            context.Nhanviens.Add(nhanvien);
            context.SaveChanges();

            return RedirectToAction("Index", "NhanVien");
        }


        public IActionResult Edit(int id)
        {
            var nhanvien = context.Nhanviens.Find(id);
            if (nhanvien == null)
            {
                return RedirectToAction("Index", "NhanVien");
            }

            // Map NhanVien entity to NhanVienDto
            var nhanVienDto = new NhanVienDto
            {
                ten_nv = nhanvien.ten_nv,
                sdt_nv = nhanvien.sdt_nv,
                email_nv = nhanvien.email_nv,
                chucvu_nv = nhanvien.chucvu_nv,
                matkhau_nv = nhanvien.matkhau_nv,
            };

            ViewData["NhanVienId"] = nhanvien.id_nv;

            return View(nhanVienDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, NhanVienDto nhanVienDto)
        {
            var nhanvien = context.Nhanviens.Find(id);

            if (nhanvien == null)
            {
                return RedirectToAction("Index", "NhanVien");
            }

            if (!ModelState.IsValid)
            {
                ViewData["NhanVienId"] = nhanvien.id_nv;
                return View(nhanVienDto);
            }

            // Update employee details
            nhanvien.ten_nv = nhanVienDto.ten_nv;
            nhanvien.sdt_nv = nhanVienDto.sdt_nv;
            nhanvien.email_nv = nhanVienDto.email_nv;
            nhanvien.chucvu_nv = nhanVienDto.chucvu_nv;

            // Update password only if a new password is provided
            if (!string.IsNullOrEmpty(nhanVienDto.matkhau_nv))
            {
                nhanvien.matkhau_nv = nhanVienDto.matkhau_nv;
            }

            context.SaveChanges();

            return RedirectToAction("Index", "NhanVien");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var nhanvien = context.Nhanviens.Find(id);
            if (nhanvien == null)
            {
                return RedirectToAction("Index", "NhanVien");
            }

            context.Nhanviens.Remove(nhanvien);
            context.SaveChanges();

            return RedirectToAction("Index", "NhanVien");
        }
    }
}
