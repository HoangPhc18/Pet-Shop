using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetCare.Models;
using PetCare.Services;
using System;

namespace PetCare.Controllers.Admin
{
    public class LoaiSPController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;
        public LoaiSPController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        // GET: LoaiSPController
        public ActionResult Index(Sanpham_Loai loaisp)
        {
            var loai = context.Sanpham_loais.ToList();
            return View(loai);
        }

        // GET: LoaiSPController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoaiSPController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sanpham_loaiDto loaiDto)
        {
            if (loaiDto.hinh_anh == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(loaiDto);
            }

            //Lưu hình ảnh
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(loaiDto.hinh_anh!.FileName);
            string imageFullPath = environment.WebRootPath + "/assets/loaisp/" + newFileName;
            using (var steam = System.IO.File.Create(imageFullPath))
            {
                loaiDto.hinh_anh.CopyTo(steam);
            }

            //Thêm vô CSDL
            Sanpham_Loai loaisp = new Sanpham_Loai
            {
                name = loaiDto.name,
                hinh_anh = newFileName
            };

            context.Sanpham_loais.Add(loaisp);
            context.SaveChanges();

            return RedirectToAction("Index", "LoaiSP");
        }

        // GET: LoaiSPController/Edit/5
        public ActionResult Edit(int id)
        {
            var loai_sp = context.Sanpham_loais.Find(id);
            if (loai_sp == null)
            {
                return RedirectToAction("Index", "LoaiSP");
            }

            //tạo sanphamDto lên sanpham
            var loaiDto = new Sanpham_loaiDto()
            {
                name = loai_sp.name,
            };

            ViewData["Id"] = loai_sp.id;
            ViewData["ImageFileName"] = loai_sp.hinh_anh;

            return View(loaiDto);
        }

        // POST: LoaiSPController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Sanpham_loaiDto dto )
        {
            var loai = context.Sanpham_loais.Find(id);

            if (loai == null)
            {
                return RedirectToAction("Index", "LoaiSP");
            }

            if (!ModelState.IsValid)
            {
                ViewData["Id"] = loai.id;
                ViewData["ImageFileName"] = loai.hinh_anh;

                return View(dto);
            }

            string newFileName = loai.hinh_anh;
            if (dto.hinh_anh != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(dto.hinh_anh.FileName);
                string imageFullPath = environment.WebRootPath + "/assets/loaisp/" + newFileName;
                using (var steam = System.IO.File.Create(imageFullPath))
                {
                    dto.hinh_anh.CopyTo(steam);
                }

                string oldImageFullPath = environment.WebRootPath + "/assets/loaisp/" + loai.hinh_anh;
                System.IO.File.Delete(oldImageFullPath);
            }

            loai.name = dto.name;
            loai.hinh_anh = newFileName;

            context.SaveChanges();

            return RedirectToAction("Index", "LoaiSP");
        }

        public ActionResult Delete(int id)
        {
            var loai = context.Sanpham_loais.Find(id);
            if (loai == null)
            {
                return RedirectToAction("Index", "LoaiSP");
            }

            string imageFullPath = environment.WebRootPath + "/assets/loaisp/" + loai.hinh_anh;
            System.IO.File.Delete(imageFullPath);

            context.Sanpham_loais.Remove(loai);
            context.SaveChanges();

            return RedirectToAction("Index", "LoaiSP");
        }
    }
}
