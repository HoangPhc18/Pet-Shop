using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using petCare.Models;
using PetCare.Models;
using PetCare.Models.Authentication;
using PetCare.Services;

namespace PetCare.Controllers
{
    //Quản lý tài khoản
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public AccountController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Login()
        {
            // Store the previous page URL before showing the login page
            if (HttpContext.Session.GetString("PreviousUrl") == null)
            {
                HttpContext.Session.SetString("PreviousUrl", HttpContext.Request.Headers["Referer"].ToString());
            }

            if (HttpContext.Session.GetInt32("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Shop");
            }
        }
        [HttpPost]
        public IActionResult Login(VMTaiKhoan taikhoan)
        {
            if (HttpContext.Session.GetInt32("Username") == null)
            {
                var userKH = context.Khachhangs.FirstOrDefault(kh => (kh.sdt_kh.Equals(taikhoan.Username) == true) || (kh.diachi_kh.Equals(taikhoan.Username) && kh.matkhau == taikhoan.Password));

                var userNV = context.Nhanviens.FirstOrDefault(nv => (nv.sdt_nv.Equals(taikhoan.Username) == true) || (nv.email_nv.Equals(taikhoan.Username) && nv.matkhau_nv == taikhoan.Password));

                if (userKH != null)
                {
                    HttpContext.Session.SetInt32("Username", userKH.id_kh);

                    // Check the previous URL, but do not allow redirect to Admin page if the user is a customer (KH)
                    var previousUrl = HttpContext.Session.GetString("PreviousUrl");
                    if (!string.IsNullOrEmpty(previousUrl) && !previousUrl.Contains("/Admin"))
                    {
                        return Redirect(previousUrl); // Redirect to previous URL if valid and not an Admin page
                    }

                    return RedirectToAction("Index", "Shop"); // Default redirect to the Shop page
                }

                if (userNV != null)
                {
                    HttpContext.Session.SetInt32("Username", userNV.id_nv);
                    HttpContext.Session.SetString("ChucVu", userNV.chucvu_nv);

                    return RedirectToAction("Index", "Admin");
                }
            }
            ModelState.AddModelError("", "Tài khoản không tìm thấy");
            return View(taikhoan);
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Register(KhachhangDto khachhangDto)
        {
            if(context.Khachhangs.Any(kh => kh.diachi_kh == khachhangDto.diachi_kh || kh.sdt_kh == khachhangDto.sdt_kh))
            {
                ModelState.AddModelError("", "Email or Phone already exists.");
                return View(khachhangDto);
            }

            var khachhang = new Khachhang
            {
                ten_kh = khachhangDto.ten_kh,
                sdt_kh = khachhangDto.sdt_kh,
                diachi_kh = khachhangDto.diachi_kh,
                matkhau = khachhangDto.matkhau_kh
            };

            context.Add(khachhang);
            await context.SaveChangesAsync();

            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Profile()
        {
            int? userId = HttpContext.Session.GetInt32("Username");
            if (userId == null)
            {
                HttpContext.Session.Clear();
            }
            var user = await context.Khachhangs.FindAsync(userId);

            ViewData["UserId"] = user?.id_kh;
            ViewData["UserName"] = user?.ten_kh;

            var khachhangDto = new KhachhangDto
            {
                ten_kh = user.ten_kh,
                sdt_kh = user.sdt_kh,
                diachi_kh = user.diachi_kh
            };

            return View(khachhangDto);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(KhachhangDto dto)
        {
            int? userId = HttpContext.Session.GetInt32("Username");

            var khachhang = await context.Khachhangs.FindAsync(userId);

            if (khachhang == null)
            {
                return RedirectToAction("Profile", "Account");
            }

            if (!ModelState.IsValid)
            {
                ViewData["UserId"] = khachhang.id_kh;
                ViewData["UserName"] = khachhang.ten_kh;
                return View(dto);
            }

            // Update employee details
            khachhang.ten_kh = dto.ten_kh;
            khachhang.sdt_kh = dto.sdt_kh;
            khachhang.diachi_kh = dto.diachi_kh;

            // Update password only if a new password is provided
            if (!string.IsNullOrEmpty(dto.matkhau_kh))
            {
                khachhang.matkhau = dto.matkhau_kh;
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Profile", "Account");
        }

        public IActionResult ThuCung()
        {
            int? userId = HttpContext.Session.GetInt32("Username");
            var khachhang = context.Khachhangs.Find(userId);
            ViewData["UserId"] = khachhang.id_kh;
            ViewData["UserName"] = khachhang.ten_kh;
            if (khachhang == null)
            {
                return NotFound();
            }

            var thucungs = context.Thucungs.Where(t => t.id_kh == userId).ToList();

            ViewData["IDKH"] = userId; // Pass the ID to the view

            var viewModel = new VMKhachHangThuCung
            {
                Khachhang = khachhang,
                Thucungs = thucungs
            };

            return View(viewModel);
        }

        public IActionResult CreateTC()
        {
            int? userId = HttpContext.Session.GetInt32("Username");
            var khachhang = context.Khachhangs.Find(userId);
            ViewData["UserId"] = khachhang.id_kh;
            ViewData["UserName"] = khachhang.ten_kh;
            ViewData["IDKH"] = khachhang.id_kh;
            return View();
        }

        [HttpPost]
        public IActionResult CreateTC(ThucungDto thucungdto)
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

            return RedirectToAction("ThuCung", "Account");
        }

        public async Task<IActionResult> Orders()
        {
            int? userId = HttpContext.Session.GetInt32("Username");

            if (userId == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
            }

            var user = await context.Khachhangs.FindAsync(userId);

            if (user == null)
            {
                TempData["Error"] = "Customer not found.";
                return RedirectToAction("Index", "Home");
            }

            ViewData["UserId"] = user.id_kh;
            ViewData["UserName"] = user.ten_kh;

            // Query orders for the logged-in user
            var donhang = await context.Donhangs
                .Where(dh => dh.id_kh == userId) // Filter by userId
                .Include(dh => dh.Khachhang) // Include Khachhang for related data
                .Select(dh => new VMChitietdh
                {
                    id_dh = dh.id_dh,
                    sdt_kh = dh.Khachhang.sdt_kh,
                    diachi_giao = dh.diachi_giao,
                    tong_tien = dh.tong_tien,
                    ngay_tao = dh.CreateAt,
                    trang_thai = dh.trang_thai,
                    ghi_chu = dh.ghi_chu,
                    ma_dh = dh.ma_dh,
                })
                .ToListAsync();

            return View(donhang); // Pass the filtered orders to the view
        }
        public async Task<ActionResult> Details(int id)
        {
            int? userId = HttpContext.Session.GetInt32("Username");

            if (userId == null)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
            }

            var user = await context.Khachhangs.FindAsync(userId);
            ViewData["UserId"] = user.id_kh;
            ViewData["UserName"] = user.ten_kh;

            var donhang = await context.Donhangs.FirstOrDefaultAsync(dh => dh.id_dh == id);
            if (donhang == null) return NotFound("Không tìm thấy đơn hàng");

            var khachhang = await context.Khachhangs.FirstOrDefaultAsync(kh => kh.id_kh == donhang.id_kh);
            var nhanVien = await context.Nhanviens.FirstOrDefaultAsync(nv => nv.id_nv == donhang.id_nv);

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
            if (user != null)
            {
                nhanvien = nhanVien.ten_nv;
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

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("Username");
                if (userId == null)
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }

                var order = await context.Donhangs.FirstOrDefaultAsync(dh => dh.id_dh == id && dh.id_kh == userId);

                if (order == null)
                {
                    TempData["Error"] = "Order not found or you do not have permission to cancel it.";
                    return RedirectToAction("Orders");
                }

                if (order.trang_thai == "Đã giao" || order.trang_thai == "Hủy đơn")
                {
                    TempData["Error"] = "This order cannot be canceled.";
                    return RedirectToAction("Orders");
                }

                order.trang_thai = "Hủy đơn";

                await context.SaveChangesAsync();

                TempData["Success"] = "Order has been canceled.";
                return RedirectToAction("Orders");
            }
            catch (Exception ex)
            {
                // Log the error (using a logging framework)
                Console.WriteLine(ex.Message);
                TempData["Error"] = "An error occurred while trying to cancel the order.";
                return RedirectToAction("Orders");
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            HttpContext.Session.Remove("Username");
            HttpContext.Session.Remove("PreviousUrl"); // Clear the stored previous URL
            return RedirectToAction("Login", "Account");
        }


    }
}
