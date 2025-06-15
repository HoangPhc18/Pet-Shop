using Microsoft.AspNetCore.Mvc;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class DichVuController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public DichVuController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var dichvu = context.DichVus.OrderBy(dv => dv.id_dichvu).ToList();
            return View(dichvu);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(DichVuDto dichvuDto)
        {
            if (!ModelState.IsValid)
            {
                return View(dichvuDto);
            }

            var dichvu = new DichVu
            {
                ten_dichvu = dichvuDto.ten_dichvu,
                loai_dichvu = dichvuDto.loai_dichvu
            };

            context.DichVus.Add(dichvu);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Edit Actions
        public IActionResult Edit(int id)
        {
            var dichvu = context.DichVus.Find(id);
            if (dichvu == null)
            {
                return RedirectToAction("Index");
            }

            var dichvuDto = new DichVuDto
            {
                ten_dichvu = dichvu.ten_dichvu,
                loai_dichvu = dichvu.loai_dichvu
            };

            ViewData["dichvuId"] = dichvu.id_dichvu;
            return View(dichvuDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, DichVuDto dichvuDto)
        {
            var dichvu = context.DichVus.Find(id);
            if (dichvu == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewData["dichvuId"] = dichvu.id_dichvu;
                return View(dichvuDto);
            }

            dichvu.ten_dichvu = dichvuDto.ten_dichvu;
            dichvu.loai_dichvu = dichvuDto.loai_dichvu;

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Detail Action
        public IActionResult Detail(int id)
        {
            var dichvu = context.DichVus.Find(id);
            if (dichvu == null)
            {
                return NotFound();
            }

            var dichvucn = context.DichVu_CanNangs.Where(t => t.id_dichvu == id).ToList();
            var dichvun = context.DichVu_Ngays.Where(j => j.id_dichvu == id).ToList();

            ViewData["dichvuId"] = id;

            var viewModel = new VMDichVu
            {
                DichVu = dichvu,
                DichVu_CanNangs = dichvucn,
                DichVu_Ngays = dichvun
            };

            return View(viewModel);
        }

        // Delete Action
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dichvu = context.DichVus.Find(id);
            if (dichvu == null)
            {
                return RedirectToAction("Index");
            }

            context.DichVus.Remove(dichvu);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateDichVuCN(DichVu_CanNang dichVuCanNang)
        {
            if (ModelState.IsValid)
            {
                context.DichVu_CanNangs.Add(dichVuCanNang);
                context.SaveChanges();
                return RedirectToAction("Detail", "DichVu", new { id = dichVuCanNang.id_dichvu });
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult EditDichVuCN(int id, DichVu_CanNang updatedDichVuCanNang)
        {
            var dichVuCanNang = context.DichVu_CanNangs.Find(id);
            if (dichVuCanNang == null)
            {
                return NotFound();
            }

            dichVuCanNang.min_can_nang = updatedDichVuCanNang.min_can_nang;
            dichVuCanNang.max_can_nang = updatedDichVuCanNang.max_can_nang;
            dichVuCanNang.gia_thanh = updatedDichVuCanNang.gia_thanh;

            context.SaveChanges();
            return RedirectToAction("Detail", "DichVu", new { id = dichVuCanNang.id_dichvu });
        }
    }
}
