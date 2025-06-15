using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Services;
using PetCare.Models;

namespace PetCare.Controllers.Admin
{
    public class KhoController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public KhoController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public async Task<IActionResult> Index()
        {
            var khoHangData = await context.Khohangs
                .Include(k => k.Sanpham)
                .Select(k => new VMKhoSanPham
                {
                    id_kho = k.id_kho,
                    id_sp = k.id_sp,
                    ten_sanpham = k.Sanpham.ten_sanpham,
                    soluong = k.soluong,
                    ngaynhap = k.CreatedAt
                })
                .ToListAsync();

            return View(khoHangData);
        }

        public IActionResult Create()
        {

            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhohangDto dto)
        {
            if (!ModelState.IsValid)
            {

                return View(dto);
            }
            var Sanpham = await context.Sanphams.FirstOrDefaultAsync(sp => sp.ma_sanpham == dto.ma_sanpham);
            if (Sanpham == null)
            {
                ModelState.AddModelError("ma_sanpham", "Mã sản phẩm không tồn tại.");
                return View(dto);
            }

            var khoHang = new Khohang
            {
                id_sp = Sanpham.id_sanpham,
                soluong = dto.soluong,
                CreatedAt = DateTime.Now,
            };

            // Update the total quantity in Sanpham
            Sanpham.soluong += khoHang.soluong;

            context.Khohangs.Add(khoHang);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int idKho, int idSp)
        {
            var khoHang = await context.Khohangs
                .Include(k => k.Sanpham)
                .FirstOrDefaultAsync(k => k.id_kho == idKho && k.id_sp == idSp);

            if (khoHang == null)
            {
                return NotFound();
            }

            // Map to ViewModel for editing
            var viewModel = new VMKhoSanPham
            {
                id_kho = khoHang.id_kho,
                ten_sanpham = khoHang.Sanpham.ten_sanpham,
                soluong = khoHang.soluong
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int idKho, int idSP, VMKhoSanPham model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Fetch the Khohang record by idKho and idSP
            var khoHang = await context.Khohangs
                .FirstOrDefaultAsync(k => k.id_kho == idKho && k.id_sp == idSP);

            if (khoHang == null)
            {
                return NotFound();
            }

            // Fetch the associated Sanpham
            var sanPham = await context.Sanphams.FirstOrDefaultAsync(sp => sp.id_sanpham == idSP);
            if (sanPham == null)
            {
                ModelState.AddModelError("", "Sản phẩm không tồn tại.");
                return View(model);
            }

            // Update the Sanpham quantity based on the change
            var difference = model.soluong - khoHang.soluong;
            sanPham.soluong += difference;

            // Update Khohang quantity
            khoHang.soluong = model.soluong;

            // Save changes
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Delete(int id)
        {
            var khohang = context.Khohangs.Find(id);
            if (khohang == null)
            {
                return RedirectToAction("Index");
            }

            context.Khohangs.Remove(khohang);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
