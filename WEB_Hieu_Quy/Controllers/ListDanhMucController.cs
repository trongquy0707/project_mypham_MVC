using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB_Hieu_Quy.Controllers
{
    public class ListDanhMucController : Controller
    {
          private Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();
        // GET: ListDanhMuc
        public ActionResult Index()
        {
            var list = db.DANH_MUC_SAN_PHAM.ToList();
            return PartialView("_index",list);
        }
        public ActionResult ItemsDanhMuc(int id)
        {
            var list = db.DANH_MUC_SAN_PHAM.Where(x => x.MaDanhMuc == id).ToList();
            return View(list);
        }
        public ActionResult ListDanhMuc()
        {
            var list = db.DANH_MUC_SAN_PHAM.ToList();
            return PartialView("_listDanhMuc", list);
        }
    }
}