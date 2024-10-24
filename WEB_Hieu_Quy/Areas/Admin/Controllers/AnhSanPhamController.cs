using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB_Hieu_Quy.Areas.Admin.Controllers
{
    public class AnhSanPhamController : Controller
    {
        Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();
        public ActionResult Index(int id)
        {
            ViewBag.MaSP = id;
            var items = db.HINH_ANH.Where(x => x.MaSanPham == id).ToList();
            return View(items);
        }
        [HttpPost]
        public ActionResult ThemAnh(int MaSP, string url)
        {
            db.HINH_ANH.Add(new Models.HINH_ANH
            {
                MaSanPham = MaSP,
                HinhAnh = url,
            });
            db.SaveChanges();
            return Json(new { Success = true });
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.HINH_ANH.Find(id);
            db.HINH_ANH.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult DeleteAll(int id)
        {
            
            var items = db.HINH_ANH.Where(x => x.MaSanPham == id).ToList();
            db.HINH_ANH.RemoveRange(items);
            db.SaveChanges();
            return Json(new { successs = true });
        }
    }
}