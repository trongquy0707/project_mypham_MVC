using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using WEB_Hieu_Quy.Models;
using WEB_Hieu_Quy.Controllers;

namespace WEB_Hieu_Quy.Areas.Admin.Controllers
{
 
    public class SanPhamChiTietController : Controller
    {
        
        // GET: Admin/SanPhamChiTiet
        private Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();

        public ActionResult Index()
        {
            var list = db.SAN_PHAM_CHI_TIET.ToList();
            return View(list);
        }
        [HttpGet]
        public ActionResult Add()
        {
            var danhMucList = db.DANH_MUC_SAN_PHAM.ToList();
            ViewBag.MaDanhMuc = new SelectList(danhMucList, "MaDanhMuc", "TenDangMuc");
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(SAN_PHAM_CHI_TIET model, List<string> Images, List<int> rDefault)
        {
            var danhMucList = db.DANH_MUC_SAN_PHAM.ToList();
            ViewBag.MaDanhMuc = new SelectList(danhMucList, "MaDanhMuc", "TenDangMuc");
            try
            {
                if (ModelState.IsValid)
                {
                    if (Images != null && Images.Count > 0)
                    {
                        for (int i = 0; i < Images.Count; i++)
                        {
                            if (i + 1 == rDefault[0])
                            {
                                model.HinhAnhChinh = Images[i];
                                model.HINH_ANH.Add(new HINH_ANH
                                {
                                    MaSanPham = model.MaSP,
                                    HinhAnh = Images[i],
                                    AnhChinh = true
                                });
                            }
                            else
                            {
                                model.HINH_ANH.Add(new HINH_ANH
                                {
                                    MaSanPham = model.MaSP,
                                    HinhAnh = Images[i],
                                    AnhChinh = false
                                });
                            }
                        }
                    }
                    var sale = model.GiaBan;
                    sale = (model.GiaGoc / 100) * model.PhanTramSale;
                    model.GiaBan = (model.GiaGoc - sale);
                    model.TrangThai = 1;

                    db.SAN_PHAM_CHI_TIET.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SuaSanPham(int id, SAN_PHAM_CHI_TIET SPCT)
        {
            var danhMucList = db.DANH_MUC_SAN_PHAM.ToList();
            ViewBag.MaDanhMuc = new SelectList(danhMucList, "MaDanhMuc", "TenDangMuc");
            var obj = db.SAN_PHAM_CHI_TIET.FirstOrDefault(c => c.MaSP == id);
            return View(obj);
        }
        [HttpPost]
        public ActionResult SuaSanPham(Models.SAN_PHAM_CHI_TIET obj)
        {
            var crrobj = db.SAN_PHAM_CHI_TIET.Find(obj.MaSP);
            var danhMucList = db.DANH_MUC_SAN_PHAM.ToList();
            ViewBag.MaDanhMuc = new SelectList(danhMucList, "MaDanhMuc", "TenDangMuc");
            if (ModelState.IsValid)
            {
                

                crrobj.MaDanhMuc = obj.MaDanhMuc;
                crrobj.TenSanPham = obj.TenSanPham;
                //crrobj.HinhAnhChinh = crrobj.HinhAnhChinh;
                crrobj.MoTaChiTiet = obj.MoTaChiTiet;
                crrobj.GiaGoc = obj.GiaGoc;
                //crrobj.GiaSale = obj.GiaSale;
                var sale = (obj.GiaGoc / 100) * obj.PhanTramSale;
                crrobj.GiaBan = (obj.GiaGoc - sale);
                crrobj.IsHome = obj.IsHome;
                crrobj.IsSale = obj.IsSale;
                crrobj.TrangThai = 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Không thành công");
            return View(obj);
        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            var obj = db.SAN_PHAM_CHI_TIET.Find(id);
            if (obj != null)
            {
                db.SAN_PHAM_CHI_TIET.Remove(obj);
                db.SaveChanges();
                return Json(new { success = true });

            }
            return Json(new { success = false });
        }
        [HttpPost]
        public ActionResult IsHome(int id)
        {
            var item = db.SAN_PHAM_CHI_TIET.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, IsHome = item.IsHome });
            }
            return Json(new { success = false });

        }
        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = db.SAN_PHAM_CHI_TIET.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, issale = item.IsSale });
            }
            return Json(new { success = false });
        }

    }
}