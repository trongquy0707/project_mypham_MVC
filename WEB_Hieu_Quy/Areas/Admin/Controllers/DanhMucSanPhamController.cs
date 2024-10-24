using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static WEB_Hieu_Quy.Controllers.AccounttController;

namespace WEB_Hieu_Quy.Areas.Admin.Controllers
{
    
    public class DanhMucSanPhamController : Controller
    {
        // GET: Admin/DanhMucSanPham
        Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();
        public ActionResult Index()
        {
            var list = db.DANH_MUC_SAN_PHAM.ToList();
            return View(list);
        }
        // them moi danh muc
        [HttpGet]
        public ActionResult ThemDanhMuc()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemDanhMuc(Models.DANH_MUC_SAN_PHAM obj , string Image)
        {
            if ((string.IsNullOrEmpty(obj.TenDangMuc)))
            {
                ModelState.AddModelError("", "Bạn chưa nhập tên danh mục !!!");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    obj.AnhDanhMuc = Image;
                    db.DANH_MUC_SAN_PHAM.Add(obj);
                    db.SaveChanges();
                }
                catch { }
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        [HttpGet]

        public ActionResult SuaDanhMuc(int id)
        {
            var obj = db.DANH_MUC_SAN_PHAM.Find(id);
            return View(obj);
        }

        //[HttpPost]
        public ActionResult SuaDanhMuc(Models.DANH_MUC_SAN_PHAM obj, string Image)
        {
            if ((string.IsNullOrEmpty(obj.TenDangMuc)))
            {
                ModelState.AddModelError("", "Vui lòng nhập đầy đủ thông tin!!! (");
            }
            if (ModelState.IsValid)
            {
                var crrObj = db.DANH_MUC_SAN_PHAM.Find(obj.MaDanhMuc);
                crrObj.AnhDanhMuc = Image;
                crrObj.TenDangMuc = obj.TenDangMuc;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }
   
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            var obj = db.DANH_MUC_SAN_PHAM.Find(id);
            if (obj != null)
            {
                db.DANH_MUC_SAN_PHAM.Remove(obj);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

    }
}