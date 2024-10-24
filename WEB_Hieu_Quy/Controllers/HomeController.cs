using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using WEB_Hieu_Quy.Models;

namespace WEB_Hieu_Quy.Controllers
{
  
    public class HomeController : Controller
    {
        private Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();
        public ActionResult Index()
        {
            var items = db.SAN_PHAM_CHI_TIET.Where(x => x.IsHome).Take(12).ToList();
            
            return View(items);
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
      
    }
}
