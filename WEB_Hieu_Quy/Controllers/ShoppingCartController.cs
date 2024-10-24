using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_Hieu_Quy.Models;
using WEB_Hieu_Quy.Models.Payments;

namespace WEB_Hieu_Quy.Controllers
{
    public class ShoppingCartController : Controller
    {
        private Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            List<ShoppingCartItem> cartItems = cart != null ? cart.Items.ToList() : new List<ShoppingCartItem>();
            return View(cartItems);
        }
        [HttpGet]
        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel order)
        {
            var code = new { success = false, quy = -1, Url="" };
            if (ModelState.IsValid)
            {
             
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    HOA_DON hoadon = new HOA_DON();
                    hoadon.TenKhachHang = order.TenKhachHang;
                    hoadon.SDT = order.SoDienThoai;
                    hoadon.Email = order.Email;
                    hoadon.ThanhPho = order.ThanhPho;
                    hoadon.Quan_Huyen = order.Quan_Huyen;
                    hoadon.Xa_Phuong = order.Xa_Phuong;
                    hoadon.Ghi_Chu = order.Ghi_Chu;
                    cart.Items.ForEach(x => hoadon.HOA_DON_CHI_TIET.Add(
                        new HOA_DON_CHI_TIET
                        {
                            MaSP = x.IdSanPham,
                            SoLuongMua = x.SoLuong,
                            GiaBan = (double)x.GiaSanPham
                        }
                        ));
                    hoadon.TongTien = (double)cart.Items.Sum(x => (x.GiaSanPham * x.SoLuong));
                    hoadon.Thanh_Toan = order.Thanh_Toan;
                    hoadon.NgayMua = DateTime.Now;
                    Random rd = new Random();
                    hoadon.Code = "HD" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    db.HOA_DON.Add(hoadon);
                    db.SaveChanges();

                    //// gui mail cho khach hang
                    var stringsanpham = "";
                    var thanhtien = decimal.Zero;
                    var tongtien = decimal.Zero;
                    foreach (var items in cart.Items)
                    {
                        stringsanpham += "<tr>";
                        stringsanpham += "<td>" + items.TenSanPham + "</td>";
                        stringsanpham += "<td>" + items.SoLuong + "</td>";
                        stringsanpham += "<td>" + WEB_Hieu_Quy.Common.Common.FormatNumber(items.TongGia, 0) + "</td>";
                        stringsanpham += "</tr>";
                        tongtien += items.GiaSanPham * items.SoLuong;
                    }
                    thanhtien = tongtien + 40000;
                    string KhachHang = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    KhachHang = KhachHang.Replace("{{MaDon}}", hoadon.Code);
                    KhachHang = KhachHang.Replace("{{SanPham}}", stringsanpham);
                    KhachHang = KhachHang.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    KhachHang = KhachHang.Replace("{{TenKhachHang}}", hoadon.TenKhachHang);
                    KhachHang = KhachHang.Replace("{{Phone}}", hoadon.SDT);
                    KhachHang = KhachHang.Replace("{{Email}}", order.Email);
                    KhachHang = KhachHang.Replace("{{DiaChiNhanHang}}", order.Xa_Phuong + " " + order.Quan_Huyen + " " + order.ThanhPho);
                    KhachHang = KhachHang.Replace("{{ThanhTien}}", WEB_Hieu_Quy.Common.Common.FormatNumber(thanhtien, 0));
                    KhachHang = KhachHang.Replace("{{TongTien}}", WEB_Hieu_Quy.Common.Common.FormatNumber(tongtien, 0));
                    KhachHang = KhachHang.Replace("{{GhiChu}}", order.Ghi_Chu);
                    WEB_Hieu_Quy.Common.Common.SendMail("Mỹ Phẩm Quý && Hiếu", "Đơn hàng #" + hoadon.Code, KhachHang.ToString(), order.Email);
                    cart.ClearCart();
                    //var Url = UrlPayment(order.Thanh_Toan_VnPay, hoadon.Code);
                    code = new { success = true, quy = order.Thanh_Toan, Url = "" };
                    if (order.Thanh_Toan == 0)
                    {
                        var url = UrlPayment(order.Thanh_Toan, hoadon.Code);
                        code = new { success = true, quy = order.Thanh_Toan, Url = url };
                       
                    }

                    //return RedirectToAction("CheckOutSucces");
                }
            }
            return Json(code);
        }



        public ActionResult partial_thanhtoan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                return PartialView(cart.Items);
            }
            return PartialView();
        }
        [HttpPost]
        public ActionResult Detele(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var items = cart.Items.FirstOrDefault(x => x.IdSanPham == id);
                if (items != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count };
                }
            }
            return Json(code);
        }
        [HttpGet]
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = 0 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
        {
            var code = new { Success = false, msg = "", code = -1, count = 0 };
            var checkSp = db.SAN_PHAM_CHI_TIET.FirstOrDefault(x => x.MaSP == id);
            if (checkSp != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    IdSanPham = checkSp.MaSP,
                    TenSanPham = checkSp.TenSanPham,
                    AnhSanPham = checkSp.HinhAnhChinh,
                    SoLuong = quantity,
                };
                item.GiaSanPham = (decimal)checkSp.GiaGoc;
                if (checkSp.GiaBan > 0)
                {
                    item.GiaSanPham = (decimal)checkSp.GiaBan;
                }
                item.TongGia = item.SoLuong * item.GiaSanPham;
                cart.AddtoCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true, msg = "Thêm giỏ hàng thành công!!", code = 1, count = cart.Items.Count };
            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { Success = true });
            }
            return Json(new { Success = false });
        }

    
        public ActionResult CheckOutSucces()
        {
            return View();
        }
        public ActionResult VnpayReturn ()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.HOA_DON.FirstOrDefault(x => x.Code == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.Status = 2;//đã thanh toán
                            db.HOA_DON.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString() + "Nghìn đồng";
                 
                }
            }
            //var a = UrlPayment(0, "DH3574");
            return View();
        }
        #region Thanh toán vnpay
        public string UrlPayment(int TypePaymentVN, string orderCode)
        {
            var urlPayment = "";
            var order = db.HOA_DON.FirstOrDefault(x => x.Code == orderCode);
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order.TongTien * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order.NgayMua.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng :" + order.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
            return urlPayment;
        }
        #endregion

    }
}