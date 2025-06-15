using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class KhachHangController : Controller
    {
        private readonly ApplicationDbContext context;

        public KhachHangController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // Index Action

        public async Task<IActionResult> Index(string? ten_kh, string? sdt_kh, string? diachi_kh, string? sortBy)
        {
            // Populate sorting options
            ViewBag.SortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Sắp Xếp Theo --" },
                new SelectListItem { Value = "name_asc", Text = "Tên (A-Z)" },
                new SelectListItem { Value = "name_desc", Text = "Tên (Z-A)" },
                new SelectListItem { Value = "phone_asc", Text = "Số Điện Thoại (Tăng)" },
                new SelectListItem { Value = "phone_desc", Text = "Số Điện Thoại (Giảm)" },
            };

            // Filtering
            var query = context.Khachhangs.AsQueryable();

            if (!string.IsNullOrEmpty(ten_kh))
                query = query.Where(kh => kh.ten_kh.Contains(ten_kh));

            if (!string.IsNullOrEmpty(sdt_kh))
                query = query.Where(kh => kh.sdt_kh.Contains(sdt_kh));

            if (!string.IsNullOrEmpty(diachi_kh))
                query = query.Where(kh => kh.diachi_kh.Contains(diachi_kh));

            // Sorting
            query = sortBy switch
            {
                "name_asc" => query.OrderBy(kh => kh.ten_kh),
                "name_desc" => query.OrderByDescending(kh => kh.ten_kh),
                "phone_asc" => query.OrderBy(kh => kh.sdt_kh),
                "phone_desc" => query.OrderByDescending(kh => kh.sdt_kh),
                _ => query.OrderBy(kh => kh.id_kh),
            };

            var khachHangList = await query.ToListAsync();
            return View(khachHangList);
        }


        // Create Actions
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(KhachhangDto khachhangDto)
        {
            if (!ModelState.IsValid)
            {
                return View(khachhangDto);
            }

            var khachhang = new Khachhang
            {
                ten_kh = khachhangDto.ten_kh,
                sdt_kh = khachhangDto.sdt_kh,
                diachi_kh = khachhangDto.diachi_kh,
                matkhau = khachhangDto.matkhau_kh
            };

            context.Khachhangs.Add(khachhang);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Edit Actions
        public IActionResult Edit(int id)
        {
            var khachhang = context.Khachhangs.Find(id);
            if (khachhang == null)
            {
                return RedirectToAction("Index");
            }

            var khachhangDto = new KhachhangDto
            {
                ten_kh = khachhang.ten_kh,
                sdt_kh = khachhang.sdt_kh,
                diachi_kh = khachhang.diachi_kh
            };

            ViewData["khachhangId"] = khachhang.id_kh;
            return View(khachhangDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, KhachhangDto khachhangDto)
        {
            var khachhang = context.Khachhangs.Find(id);
            if (khachhang == null)
            {
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                ViewData["khachhangId"] = khachhang.id_kh;
                return View(khachhangDto);
            }

            khachhang.ten_kh = khachhangDto.ten_kh;
            khachhang.sdt_kh = khachhangDto.sdt_kh;
            khachhang.diachi_kh = khachhangDto.diachi_kh;

            if (!string.IsNullOrEmpty(khachhangDto.matkhau_kh))
            {
                khachhang.matkhau = khachhangDto.matkhau_kh;
            }

            context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Detail Action
        public IActionResult Detail(int id)
        {
            var khachhang = context.Khachhangs.Include(kh => kh.Thucungs).FirstOrDefault(kh => kh.id_kh == id);

            if (khachhang == null)
            {
                TempData["Error"] = "Khách hàng không tồn tại.";
                return RedirectToAction("Index");
            }

            var viewModel = new VMKhachHangThuCung
            {
                Khachhang = khachhang,
                Thucungs = khachhang.Thucungs.ToList()
            };

            return View(viewModel);
        }



        // Delete Action
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var khachhang = context.Khachhangs.Find(id);
            if (khachhang == null)
            {
                return RedirectToAction("Index");
            }

            context.Khachhangs.Remove(khachhang);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
