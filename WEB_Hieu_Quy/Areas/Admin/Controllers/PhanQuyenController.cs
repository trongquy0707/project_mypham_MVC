using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_Hieu_Quy.Models;
using static WEB_Hieu_Quy.Controllers.AccounttController;

namespace WEB_Hieu_Quy.Areas.Admin.Controllers
{
  
    public class PhanQuyenController : Controller
    {
        Web_CosmeticsEntities db = new Web_CosmeticsEntities();

        // GET: Admin/PhanQuyen
        public ActionResult Index()
        {
            var items = db.CHUC_VU.ToList();
            return View(items);
        }
        public ActionResult ThemQuyen()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemQuyen(CHUC_VU obj)
        {
            db.CHUC_VU.Add(obj);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        [HttpGet]
        public ActionResult SuaQuyen(int id)
        {
            var item = db.CHUC_VU.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaQuyen(CHUC_VU obj, int id)
        {
            if (ModelState.IsValid)
            {
                var items = db.CHUC_VU.FirstOrDefault(x => x.MaChucVu == id);
                items.TenChucVu = obj.TenChucVu;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(obj);
        }

    }
}