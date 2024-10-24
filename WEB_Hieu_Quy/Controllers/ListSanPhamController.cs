using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB_Hieu_Quy.Controllers
{
    public class ListSanPhamController : Controller
    {
        WEB_Hieu_Quy.Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();
        // GET: SanPham
        public ActionResult FlashSale()
        {
            var FlastSale = db.SAN_PHAM_CHI_TIET.Where(x => x.IsSale).Take(12).ToList();
            return PartialView("_FlashSale", FlastSale);
        }
        [HttpGet]
        public ActionResult ChiTietSanPham(int id)
        {
            var SanPhamChiTiet = db.SAN_PHAM_CHI_TIET.Find(id);
            return View(SanPhamChiTiet);
        }
        public ActionResult SanPhamTheoDanhMuc(int id)
        {
            var items = db.SAN_PHAM_CHI_TIET.Where(x => x.MaDanhMuc == id).ToList();
            return View(items);

        }
        public ActionResult SanPhamChung(string searchString)
        {
            var items = db.SAN_PHAM_CHI_TIET.Where(x => x.IsHome);

            // Nếu có chuỗi tìm kiếm, thêm điều kiện tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(x => x.TenSanPham.Contains(searchString));
            }

            return View(items.ToList());
        }
        //public ActionResult SanPhamTheoGia(double giamin , double giamax)
        //{
        //    var item = db.SAN_PHAM_CHI_TIET.Where(x => x.GiaBan >= giamin && x.GiaBan <= giamax).ToList();
        //    return View(item);
        //}
    }
}