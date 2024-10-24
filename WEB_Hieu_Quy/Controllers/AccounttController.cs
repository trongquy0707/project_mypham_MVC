using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using WEB_Hieu_Quy.Models;

namespace WEB_Hieu_Quy.Controllers
{
    public class AccounttController : Controller
    {
        // GET: Accountt

        Web_CosmeticsEntities db = new Web_CosmeticsEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(AccountViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Models.TAI_KHOAN checktrung = db.TAI_KHOAN.FirstOrDefault(x => x.Email == obj.Email);
                if (obj.MatKhau != obj.Retunr_MatKhau)
                {
                    //ModelState.AddModelError("Retunr_MatKhau", "Mật khẩu không trùng !!");
                    
                    TempData["loi"] = "Mật khẩu không trùng !!";
                    return View(obj);
                }
                else if(checktrung != null)
                {
                    TempData["checkTrug"] = "Email đã được sử dụng !!";
                }
                else
                {
                    // Hash mật khẩu
                    string hashedPassword = Crypto.Hash(obj.MatKhau);
                    // Tạo đối tượng TAI_KHOAN
                    TAI_KHOAN tk = new TAI_KHOAN();
                    tk.Email = obj.Email;
                    tk.TenDangNhap = obj.TenDangNhap;
                    tk.MatKhau = hashedPassword;
                    tk.SDT = obj.So_dien_Thoai;
                    tk.MaChucVu = 2;

                    try
                    {

                        db.TAI_KHOAN.Add(tk);
                        db.SaveChanges();
                        return RedirectToAction("Dangnhap");
                    }
                    catch (Exception ex)
                    {

                        ModelState.AddModelError("", "Có lỗi xảy ra khi lưu dữ liệu.");

                        return View(obj);
                    }
                }
            }
            return View(obj);
        }


        public ActionResult Dangnhap(TAI_KHOAN obj, string ReturnURL)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string hashedPassword = Crypto.Hash(obj.MatKhau);
                    Models.TAI_KHOAN check = db.TAI_KHOAN.FirstOrDefault(m => m.Email == obj.Email && m.MatKhau == hashedPassword);
                    if (check != null)
                    {
                        FormsAuthentication.SetAuthCookie(check.TenDangNhap, false);

                        var authTicket = new FormsAuthenticationTicket(
                        1,                             // version
                        check.TenDangNhap,             // user name
                        DateTime.Now,                  // issue time
                        DateTime.Now.AddSeconds(60),   // expiration
                        false,                         // persistent
                        check.MaChucVu.ToString()      // user data (MaChucVu)
                    );
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
                        if (check.MaChucVu == 2)
                        {
                            Session["TenDangNhap"] = check.TenDangNhap;
                            Session["MaUser"] = check.MaUser;

                            return RedirectToAction("Index", "Home");
                        }
                        else if (check.MaChucVu == 1)
                        {
                            return RedirectToAction("Index", "Admin/SanPhamChiTiet");
                        }
                    }
                    else
                    {
                        TempData["loidangnhap"] = "Đăng nhập thất bại";
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng nhập!");
                }
            }
            return View(obj);
        }

        [CustomAuthorize(1)]
        public ActionResult AdminOnly()
        {
            // Action chỉ dành cho admin với MaChucVu = 1

            return RedirectToAction("Index", "Admin/BaoCaoDoanhThu");
        }

        [CustomAuthorize(2)]
        public ActionResult UserOnly()
        {
            // Action chỉ dành cho người dùng với MaChucVu = 2
            return RedirectToAction("Index", "Home");
        }

        public class CustomAuthorizeAttribute : AuthorizeAttribute
        {
            private readonly int _requiredMaChucVu;

            public CustomAuthorizeAttribute(int requiredMaChucVu)
            {
                _requiredMaChucVu = requiredMaChucVu;
            }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    return false;
                }

                var userData = ((FormsIdentity)httpContext.User.Identity).Ticket.UserData;
                int maChucVu;
                if (int.TryParse(userData, out maChucVu))
                {
                    return maChucVu == _requiredMaChucVu;
                }

                return false;
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
        }

        public ActionResult QuenMK(TAI_KHOAN obj, string emaill)
    {
        if (emaill != null)
        {
            var check = db.TAI_KHOAN.FirstOrDefault(x => x.Email == emaill);
            if (check != null)
            {
                Random rd = new Random();
                string password_new = "MK" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                string hashedPassword = Crypto.Hash(password_new);
                check.MatKhau = hashedPassword;
                db.SaveChanges();
                string KhachHang = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/QuenMK.html"));
                KhachHang = KhachHang.Replace("{{Mat_Khau_Moi}}", password_new);
                WEB_Hieu_Quy.Common.Common.SendMail("Mỹ Phẩm Quý && Hiếu", password_new, KhachHang.ToString(), check.Email);
                return RedirectToAction("Dangnhap");
            }
            else
            {
                ViewBag.ErrorMessage = "Không tìm thấy Email trong hệ thống.";
            }
        }
        return View();
    }

    public ActionResult ThongTinTK(int id)
    {
        var list = db.TAI_KHOAN.Find(id);
        return View(list);
    }
    public ActionResult logout()
    {
        Session.Clear();
        //FormsAuthentication.SignOut();
        return RedirectToAction("Dangnhap", "Accountt");
    }
    public ActionResult ThongTinDangNhap(int id, TAI_KHOAN obj, string matkhaumoi, string return_mk, string Mkcu)
    {
        //var list = db.TAI_KHOAN.Find(id);
        if (obj.MatKhau == Mkcu)
        {

        }
        return View();
    }
    public ActionResult test()
    {
        return View();
    }

}
}