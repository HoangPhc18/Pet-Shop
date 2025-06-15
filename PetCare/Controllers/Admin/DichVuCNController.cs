using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using petCare.Models;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class DichVuCNController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public DichVuCNController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        // GET: DichVuCNController/Create
        public IActionResult Create(int id)
        {
            ViewData["IDDV"] = id;
            return View();
        }

        // POST: DichVuCNController/Create
        [HttpPost]
        public IActionResult Create(DichVu_CanNangDto dv_cndto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IDDV"] = dv_cndto.id_dichvu; // Preserve IDKH in case of errors
                return View(dv_cndto);
            }

            var dichvu_cns = new DichVu_CanNang
            {
                id_dichvu = dv_cndto.id_dichvu,
                min_can_nang = dv_cndto.min_can_nang,
                max_can_nang = dv_cndto.max_can_nang,
                gia_thanh = dv_cndto.gia_thanh
            };

            context.DichVu_CanNangs.Add(dichvu_cns);
            context.SaveChanges();

            return RedirectToAction("Detail", "DichVu", new { id = dichvu_cns.id_dichvu });
        }

        // GET: DichVuCNController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DichVuCNController/Edit/5
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

        // GET: DichVuCNController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DichVuCNController/Delete/5
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
