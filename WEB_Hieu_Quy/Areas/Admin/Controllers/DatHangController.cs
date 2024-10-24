using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_Hieu_Quy.Models;
using static WEB_Hieu_Quy.Controllers.AccounttController;

namespace WEB_Hieu_Quy.Areas.Admin.Controllers
{
 
    public class DatHangController : Controller
    {
      
        Web_CosmeticsEntities db = new Web_CosmeticsEntities();
        public ActionResult Index()
        {
            var items = db.HOA_DON.OrderByDescending(x => x.NgayMua).ToList();
            return View(items);
        }
        public ActionResult detailOrder(int id)
        {
            var item = db.HOA_DON.Find(id);
            return View(item);
        }
        public ActionResult ListOrder(int id)
        {
            var item = db.HOA_DON_CHI_TIET.Where(x=>x.HOA_DON.MaHD == id);
            return View(item);
            
        }
        public ActionResult CapNhatTT(int id, int trangthai)
        {
            var item = db.HOA_DON.Find(id);
            if (item != null)
            {
                db.HOA_DON.Attach(item);
                item.Thanh_Toan = trangthai;
                db.Entry(item).Property(x => x.Thanh_Toan).IsModified = true;
                db.SaveChanges();
                return Json(new { message = "Success", Success = true });
            }
            return Json(new { message = "Unsuccess", Success = false });
        }
    }
}