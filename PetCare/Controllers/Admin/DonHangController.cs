using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class DonHangController : Controller
    {

        private readonly ApplicationDbContext context;
        public DonHangController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: DonHangController
        public async Task<IActionResult> Index(string? ma_dh, string? ngaydat, string? sortOption)
        {
            // Populate sorting options
            ViewBag.SortOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Trạng Thái --" },
                new SelectListItem { Value = "Đã tạo đơn", Text = "Đã tạo đơn" },
                new SelectListItem { Value = "Đang xử lý", Text = "Đang xử lý" },
                new SelectListItem { Value = "Đang giao hàng", Text = "Đang giao hàng" },
                new SelectListItem { Value = "Hoàn thành", Text = "Hoàn thành" },
                new SelectListItem { Value = "Hủy đơn", Text = "Hủy đơn" },
            };

            var donhang = context.Donhangs.Include(dh => dh.Khachhang).Select(dh => new VMChitietdh
            {
                id_dh = dh.id_dh,
                ten_kh = dh.Khachhang.ten_kh,
                diachi_giao = dh.diachi_giao,
                tong_tien = dh.tong_tien,
                ngay_tao = dh.CreateAt,
                trang_thai = dh.trang_thai,
                ghi_chu = dh.ghi_chu,
                ma_dh = dh.ma_dh,
            });

            var query = donhang.AsQueryable();
            if (!string.IsNullOrEmpty(ma_dh))
                query = query.Where(ma => ma.ma_dh.Contains(ma_dh));

            if (!string.IsNullOrEmpty(ngaydat))
                query = query.Where(ngay => ngay.ngay_tao.ToString().Contains(ngaydat));

            // Apply sorting if a sort option is selected
            if (!string.IsNullOrEmpty(sortOption))
            {
                query = donhang.Where(a => a.trang_thai == sortOption);
            }

            // Execute the query
            var sortedOrders = await query.ToListAsync();

            return View(sortedOrders);
        }

        public async Task<IActionResult> UpdateStatus(int id)
        {
            int? userId = HttpContext.Session.GetInt32("Username");
            var order = context.Donhangs.Find(id); 
            if (order != null)
            {
                ViewBag.CurrentTrangThai = order.trang_thai;
            }

            ViewBag.TrangThaiList = new List<string>
            {
                "Đã tạo đơn",
                "Đang xử lý",
                "Đang giao hàng",
                "Hoàn thành",
                "Hủy đơn"
            };

            var user = await context.Nhanviens.FindAsync(userId);

            var trangthaidh = new VMTrangThaiDH
            {
                id_dh = order.id_dh,
                id_nv = user.id_nv,
                trang_thai = order.trang_thai,
                ma_donhang = order.ma_dh
            };

            return View(trangthaidh);
        }

        //Cập nhật trạng thái đơn
        [HttpPost]
        public IActionResult UpdateStatus(VMTrangThaiDH vm)
        {
            var order = context.Donhangs.Find(vm.id_dh);
            if (order != null)
            {
                order.id_nv = vm.id_nv;
                order.trang_thai = vm.trang_thai;

                if (vm.trang_thai == "Hủy Đơn" && !string.IsNullOrWhiteSpace(vm.ghi_chu))
                {
                    order.ghi_chu = vm.ghi_chu;
                }

                context.SaveChanges();
                TempData["Success"] = "Order status updated successfully!";
            }
            else
            {
                TempData["Error"] = "Order not found.";
            }

            return RedirectToAction("Index");
        }

        // GET: DonHangController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var donhang = await context.Donhangs.FirstOrDefaultAsync(dh => dh.id_dh == id);
            if (donhang == null) return NotFound("Không tìm thấy đơn hàng");

            var khachhang = await context.Khachhangs.FirstOrDefaultAsync(kh => kh.id_kh == donhang.id_kh);
            var user = await context.Nhanviens.FirstOrDefaultAsync(nv => nv.id_nv == donhang.id_nv);

            var chitietdon = await context.Chitietdons.Where(ct => ct.id_dh == id)
                .Join(context.Sanphams, ct => ct.id_sp, sp => sp.id_sanpham, (ct, sp) => new ChiTietDonHang
                {
                    id_sp = ct.id_sp,
                    ten_sp = sp.ten_sanpham,
                    so_luong = ct.soluong,
                    gia_sp = sp.thanhtien,
                    thanh_tien = ct.soluong * sp.thanhtien,
                    hinh_anh = sp.hinhanh
                }).ToListAsync();
            string nhanvien;
            if(user != null)
            {
                nhanvien = user.ten_nv;
            }
            else
            {
                nhanvien = "";
            }


            var viewModel = new VMChitietdh
            {
                id_dh = donhang.id_dh,
                ten_nv = nhanvien,
                ten_kh = khachhang.ten_kh,
                sdt_kh = khachhang.sdt_kh,
                diachi_giao = donhang.diachi_giao,
                trang_thai = donhang.trang_thai,
                tong_tien = donhang.tong_tien,
                ngay_tao = donhang.CreateAt,
                ghi_chu = donhang.ghi_chu,
                ma_dh = donhang.ma_dh,
                giohang = chitietdon
            };

            return View(viewModel);
        }

        // GET: DonHangController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DonHangController/Edit/5
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

        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the order along with its related Chitietdon entities
            var order = await context.Donhangs
                .Include(d => d.Chitietdons) // Include related Chitietdons
                .FirstOrDefaultAsync(d => d.id_dh == id);

            if (order == null)
            {
                TempData["Error"] = "Không tìm thấy đơn hàng.";
                return RedirectToAction("Index");
            }

            // Explicitly remove all related Chitietdon records
            if (order.Chitietdons != null && order.Chitietdons.Any())
            {
                context.Chitietdons.RemoveRange(order.Chitietdons);
            }

            // Remove the Donhang
            context.Donhangs.Remove(order);

            // Save changes
            await context.SaveChangesAsync();

            TempData["Success"] = "Đơn hàng và chi tiết đơn đã được xóa.";
            return RedirectToAction("Index");
        }

    }
}
