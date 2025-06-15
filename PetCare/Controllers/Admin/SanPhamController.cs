using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetCare.Models;
using PetCare.Services;

namespace PetCare.Controllers.Admin
{
    public class SanPhamController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public SanPhamController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        //public async Task<IActionResult> Index(int? LoaiId)
        //{
        //    // If LoaiId is null, default it to 0 (show all products)
        //    LoaiId = LoaiId ?? 0;

        //    // Fetch product categories
        //    var SPLoai = context.Sanpham_loais.ToList();
        //    SPLoai.Insert(0, new Sanpham_Loai { id = 0, name = "--- Chọn loại sản phẩm ---" });

        //    // Pass the category list to the view for the dropdown
        //    ViewBag.LoaiId = new SelectList(SPLoai, "id", "name", LoaiId);

        //    // Fetch products and include category data
        //    var SPData = await context.Sanphams
        //        .Include(sp => sp.sanpham_loai) // Include product categories
        //        .Select(sp => new VMSanPham
        //        {
        //            id_sanpham = sp.id_sanpham,
        //            ten_sanpham = sp.ten_sanpham,
        //            loai_sanpham = sp.sanpham_loai.name,
        //            hinhanh = sp.hinhanh,
        //            soluong = sp.soluong,
        //            ma_sanpham = sp.ma_sanpham,
        //            thanhtien = sp.thanhtien,
        //            id_loaisp = sp.sanpham_loai.id // Add the product category ID
        //        })
        //        .ToListAsync();

        //    // Filter the products based on the selected category
        //    if (LoaiId > 0)
        //    {
        //        SPData = SPData.Where(a => a.id_loaisp == LoaiId).ToList(); // Filter based on LoaiId
        //    }

        //    return View(SPData); // Return filtered list of products
        //}

        public async Task<IActionResult> Index(int? LoaiId)
        {
            LoaiId = LoaiId ?? 0;

            var SPLoai = await context.Sanpham_loais.AsNoTracking().ToListAsync();
            SPLoai.Insert(0, new Sanpham_Loai { id = 0, name = "--- Chọn loại sản phẩm ---" });

            ViewBag.LoaiId = new SelectList(SPLoai, "id", "name", LoaiId);

            var SPData = await context.Sanphams
                .AsNoTracking()
                .Include(sp => sp.sanpham_loai)
                .Select(sp => new VMSanPham
                {
                    id_sanpham = sp.id_sanpham,
                    ten_sanpham = sp.ten_sanpham,
                    loai_sanpham = sp.sanpham_loai.name,
                    hinhanh = sp.hinhanh,
                    soluong = sp.soluong,
                    ma_sanpham = sp.ma_sanpham,
                    thanhtien = sp.thanhtien,
                    id_loaisp = sp.sanpham_loai.id
                })
                .ToListAsync();

            if (LoaiId > 0)
            {
                SPData = SPData.Where(a => a.id_loaisp == LoaiId).ToList();
            }

            return View(SPData);
        }


        public IActionResult Create()
        {
            ViewBag.LoaiSP = context.Sanpham_loais.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(SanphamDto sanphamDto)
        {
            if (sanphamDto.hinhanh == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(sanphamDto);
            }

            //Lưu hình ảnh
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(sanphamDto.hinhanh!.FileName);
            string imageFullPath = environment.WebRootPath + "/assets/sanphams/" + newFileName;
            using (var steam = System.IO.File.Create(imageFullPath))
            {
                sanphamDto.hinhanh.CopyTo(steam);
            }

            //Thêm vô CSDL
            Sanpham sanpham = new Sanpham
            {
                ten_sanpham = sanphamDto.ten_sanpham,
                id_loaisp = sanphamDto.id_loaisp,
                thanhtien = sanphamDto.thanhtien,
                soluong = sanphamDto.soluong,
                hinhanh = newFileName,
                ma_sanpham = sanphamDto.ma_sanpham,
            };

            context.Sanphams.Add(sanpham);
            context.SaveChanges();

            return RedirectToAction("Index", "SanPham");
        }

        public IActionResult Edit(int id)
        {
            ViewBag.LoaiSP = context.Sanpham_loais.ToList();
            var sanpham = context.Sanphams.Find(id);
            if (sanpham == null)
            {
                return RedirectToAction("Index", "SanPham");
            }

            //tạo sanphamDto lên sanpham
            var sanphamDto = new SanphamDto()
            {
                ten_sanpham = sanpham.ten_sanpham,
                id_loaisp = sanpham.id_loaisp,
                thanhtien = sanpham.thanhtien,
                soluong = sanpham.soluong,
                ma_sanpham = sanpham.ma_sanpham,
            };

            ViewData["sanphamId"] = sanpham.id_sanpham;
            ViewData["ImageFileName"] = sanpham.hinhanh;

            return View(sanphamDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, SanphamDto sanphamDto)
        {
            var sanpham = context.Sanphams.Find(id);

            if (sanpham == null)
            {
                return RedirectToAction("Index", "SanPham");
            }

            if (!ModelState.IsValid)
            {
                ViewData["sanphamId"] = sanpham.id_sanpham;
                ViewData["ImageFileName"] = sanpham.hinhanh;

                return View(sanphamDto);
            }

            string newFileName = sanpham.hinhanh;
            if (sanphamDto.hinhanh != null)
            {
                newFileName = Guid.NewGuid() + Path.GetExtension(sanphamDto.hinhanh.FileName); // Tạo tên file duy nhất
                string imageFullPath = Path.Combine(environment.WebRootPath, "assets", "sanphams", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    sanphamDto.hinhanh.CopyTo(stream);
                }

                string oldImageFullPath = Path.Combine(environment.WebRootPath, "assets", "sanphams", sanpham.hinhanh);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }
            }


            sanpham.ten_sanpham = sanphamDto.ten_sanpham;
            sanpham.id_loaisp = sanphamDto.id_loaisp;
            sanpham.thanhtien = sanphamDto.thanhtien;
            sanpham.soluong = sanphamDto.soluong;
            sanpham.hinhanh = newFileName;
            sanpham.ma_sanpham = sanphamDto.ma_sanpham;

            context.SaveChanges();

            return RedirectToAction("Index", "SanPham");
        }

        public IActionResult Delete(int id)
        {
            var sanpham = context.Sanphams.Find(id);
            if (sanpham == null)
            {
                TempData["ErrorMessage"] = "Sản phẩm không tồn tại.";
                return RedirectToAction("Index", "SanPham");
            }

            try
            {
                string imageFullPath = Path.Combine(environment.WebRootPath, "assets", "sanphams", sanpham.hinhanh);
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }

                context.Sanphams.Remove(sanpham);
                context.SaveChanges();

                TempData["SuccessMessage"] = "Xóa sản phẩm thành công.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa sản phẩm: " + ex.Message;
            }

            return RedirectToAction("Index", "SanPham");
        }

    }
}
