using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class DichVuNgayController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        // GET: DichVuNgayController
        public DichVuNgayController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Create(int id)
        {
            ViewData["IDDV"] = id;
            return View();
        }

        // POST: DichVuNgayController/Create
        [HttpPost]
        public IActionResult Create(DichVu_NgayDto dv_ndto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IDDV"] = dv_ndto.id_dichvu; // Preserve IDKH in case of errors
                return View(dv_ndto);
            }

            var dichvu_ns = new DichVu_Ngay
            {
                id_dichvu = dv_ndto.id_dichvu,
                gia_thanh = dv_ndto.gia_thanh
            };

            context.DichVu_Ngays.Add(dichvu_ns);
            context.SaveChanges();

            return RedirectToAction("Detail", "DichVu", new { id = dv_ndto.id_dichvu });
        }

        // GET: DichVuNgayController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DichVuNgayController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DichVuNgayController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DichVuNgayController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
