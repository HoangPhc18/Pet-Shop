using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using petCare.Models;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class LichHenController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public LichHenController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public async Task<IActionResult> Index(string? ngayhen, string? sortOption)
        {
            // Populate sorting options
            ViewBag.SortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Trạng Thái --" },
                new SelectListItem { Value = "Đã xác nhận lịch", Text = "Đã xác nhận lịch" },
                new SelectListItem { Value = "Đã tiếp nhận thú cưng", Text = "Đã tiếp nhận thú cưng" },
                new SelectListItem { Value = "Đang làm dịch vụ", Text = "Đang làm dịch vụ" },
                new SelectListItem { Value = "Hoàn thành", Text = "Hoàn thành" },
                new SelectListItem { Value = "Đã báo khách hàng", Text = "Đã báo khách hàng" },
                new SelectListItem { Value = "Hủy Lịch Hẹn", Text = "Hủy Lịch Hẹn" },
            };

            // Fetch all appointments
            var appointments = context.Lichhens
                .Include(lh => lh.Khachhang)
                .Include(lh => lh.Thucung)
                .Include(lh => lh.DichVu)
                .Select(lh => new VMChitietLichHen
                {
                    id_lich = lh.id_lichhen,
                    id_kh = lh.id_kh,
                    ten_kh = lh.Khachhang.ten_kh,
                    sdt_kh = lh.Khachhang.sdt_kh,
                    id_tc = lh.id_tc,
                    ten_tc = lh.Thucung.ten_pet,
                    giong_tc = lh.Thucung.giong_pet,
                    can_nang = lh.Thucung.cannang_pet,
                    ngay_hen = lh.ngay_hen,
                    id_dichvu = lh.id_dichvu,
                    ten_dichvu = lh.DichVu.ten_dichvu,
                    ghi_chu = lh.ghi_chu,
                    trang_thai = lh.trang_thai
                });

            var query = appointments.AsQueryable();

            if (!string.IsNullOrEmpty(ngayhen))
                query = query.Where(ngay => ngay.ngay_hen.ToString().Contains(ngayhen));
            // Apply sorting if a sort option is selected
            if (!string.IsNullOrEmpty(sortOption))
            {
                appointments = appointments.Where(a => a.trang_thai == sortOption);
            }

            // Execute the query
            var sortedAppointments = await appointments.ToListAsync();

            return View(sortedAppointments);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["UserId"] = HttpContext.Session.GetInt32("Username");

            var lichhen = await context.Lichhens.FirstOrDefaultAsync(lh => lh.id_lichhen == id);
            if (lichhen == null) return NotFound("Không tìm thấy lịch hẹn");

            var khachhang = await context.Khachhangs.FirstOrDefaultAsync(kh => kh.id_kh == lichhen.id_kh);
            var thucung = await context.Thucungs.FirstOrDefaultAsync(tc => tc.id_pet == lichhen.id_tc);
            var user = await context.Nhanviens.FirstOrDefaultAsync(nv => nv.id_nv == lichhen.id_nv);
            var dichvu = await context.DichVus.FirstOrDefaultAsync(dv => dv.id_dichvu == lichhen.id_dichvu);

            string nhanvien;
            if (user != null)
            {
                nhanvien = user.ten_nv;
            }
            else
            {
                nhanvien = "";
            }

            var chitietlich = new VMChitietLichHen
                {
                    id_lich = lichhen.id_lichhen,
                    id_kh = lichhen.id_kh,
                    ten_kh = khachhang.ten_kh,
                    sdt_kh = khachhang.sdt_kh,
                    id_tc = lichhen.id_tc,
                    ten_tc = thucung.ten_pet,
                    giong_tc = thucung.giong_pet,
                    can_nang = thucung.cannang_pet,
                    id_dichvu = lichhen.id_dichvu,
                    ten_dichvu = dichvu.ten_dichvu,
                    ngay_hen = lichhen.ngay_hen,
                    ghi_chu = lichhen.ghi_chu,
                    id_nv = lichhen.id_nv,
                    ten_nv = nhanvien,
                    trang_thai = lichhen.trang_thai,
                };


            ViewBag.TrangThaiList = new List<string>
            {
                "Đã xác nhận lịch",
                "Đã tiếp nhận thú cưng",
                "Đang làm dịch vụ",
                "Hoàn thành",
                "Đã báo khách hàng",
                "Hủy Lịch Hẹn"
            };

            ViewBag.DichVuOption = context.DichVus.ToList();

            return View(chitietlich);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, VMChitietLichHen model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DichVuOption = await context.DichVus.ToListAsync();

                ViewBag.TrangThaiList = new List<string>
                {
                    "Đã xác nhận lịch",
                    "Đã tiếp nhận thú cưng",
                    "Đang làm dịch vụ",
                    "Hoàn thành",
                    "Đã báo khách hàng",
                    "Hủy Lịch Hẹn"
                };

                return View(model);
            }

            // Fetch the existing record from the database
            var lichhen = await context.Lichhens.FindAsync(id);
            if (lichhen == null)
            {
                return NotFound("Không tìm thấy lịch hẹn");
            }

            lichhen.id_kh = model.id_kh;
            lichhen.id_tc = model.id_tc;
            lichhen.id_dichvu = model.id_dichvu;
            lichhen.ngay_hen = model.ngay_hen;
            lichhen.ghi_chu = model.ghi_chu;
            lichhen.trang_thai = model.trang_thai;
            lichhen.id_nv = model.id_nv;

            try
            {
                // Save changes to the database
                context.Lichhens.Update(lichhen);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your administrator.");

                ViewBag.DichVuOption = await context.DichVus.ToListAsync();

                ViewBag.TrangThaiList = new List<string>
                {
                    "Đã xác nhận lịch",
                    "Đã tiếp nhận thú cưng",
                    "Đang làm dịch vụ",
                    "Hoàn thành",
                    "Đã báo khách hàng",
                    "Hủy Lịch Hẹn"
                };

                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            var lichhen = await context.Lichhens.FindAsync(id);

            if (lichhen == null)
                return NotFound("Không tìm thấy lịch hẹn");

            context.Lichhens.Remove(lichhen);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
