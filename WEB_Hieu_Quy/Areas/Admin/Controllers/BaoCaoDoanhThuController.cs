using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static WEB_Hieu_Quy.Controllers.AccounttController;

namespace WEB_Hieu_Quy.Areas.Admin.Controllers
{
  
    public class BaoCaoDoanhThuController : Controller
    {
        Models.Web_CosmeticsEntities db = new Models.Web_CosmeticsEntities();

        // GET: Admin/BaoCaoDoanhThu
        public ActionResult Index()
        {
            
            // Khởi tạo danh sách để lưu tổng doanh thu của từng tháng
            List<decimal> monthlyRevenues = new List<decimal>();

            // Khởi tạo biến để tính tổng doanh thu từ trước đến nay
            decimal totalRevenueFromBeginning = 0;

            // Lặp qua từng tháng trong năm
            for (int month = 1; month <= DateTime.Now.Month; month++)
            {
                // Tính ngày đầu tiên và cuối cùng của tháng
                DateTime startDateOfMonth = new DateTime(DateTime.Now.Year, month, 1);
                DateTime endDateOfMonth = startDateOfMonth.AddMonths(1).AddDays(-1);

                // Tính tổng doanh thu của tháng hiện tại
                decimal totalRevenueInMonth = CalculateTotalRevenue(startDateOfMonth, endDateOfMonth);

                // Thêm tổng doanh thu của tháng vào danh sách
                monthlyRevenues.Add(totalRevenueInMonth);

                // Cập nhật tổng doanh thu từ trước đến nay
                totalRevenueFromBeginning += totalRevenueInMonth;
            }

            // Truyền danh sách tổng doanh thu của từng tháng vào ViewBag để sử dụng trong view
            ViewBag.MonthlyRevenues = monthlyRevenues;

            // Truyền tổng doanh thu từ trước đến nay vào ViewBag để sử dụng trong view
            ViewBag.TotalRevenueFromBeginning = totalRevenueFromBeginning;

            // Tính sản phẩm được mua nhiều nhất
            //DateTime startDate = db.HOA_DON.Min(dh => dh.NgayMua).GetValueOrDefault();
            //DateTime endDate = DateTime.Now;
            //var mostBoughtProduct = CalculateMostBoughtProduct(startDate, endDate);

            // Truyền thông tin sản phẩm được mua nhiều nhất vào ViewBag để sử dụng trong view
            //ViewBag.MostBoughtProduct = mostBoughtProduct;

            // Tính tổng số lượng của mỗi sản phẩm đã bán ra từ trước đến nay
            //var totalQuantitySoldFromBeginning = CalculateTotalQuantitySoldFromBeginning();

            //// Truyền thông tin tổng số lượng của mỗi sản phẩm đã bán ra từ trước đến nay vào ViewBag để sử dụng trong view
            //ViewBag.TotalQuantitySoldFromBeginning = totalQuantitySoldFromBeginning;

            // Tính tổng số lượng đơn hàng từ trước đến nay
            var totalOrdersFromBeginning = CalculateTotalOrdersFromBeginning();

            // Truyền thông tin tổng số lượng đơn hàng từ trước đến nay vào ViewBag để sử dụng trong view
            ViewBag.TotalOrdersFromBeginning = totalOrdersFromBeginning;

            // Đếm số lượng khách hàng đã đăng ký
            //int customerCount = CountCustomers();
            //ViewBag.CustomerCount = customerCount;

            // Truyền số lượng đơn hàng đang chờ xử lý
            ViewBag.PendingOrdersCount = CountPendingOrders();

            // Truyền số lượng đơn hàng đang giao
            //ViewBag.DeliveringOrdersCount = CountDeliveringOrders();

            // Truyền số lượng đơn hàng đã hoàn thành
            ViewBag.CompletedOrdersCount = CountCompletedOrders();

            return View();
        }

        // Hàm tính tổng doanh thu trong một khoảng thời gian cụ thể
        public decimal CalculateTotalRevenue(DateTime startDate, DateTime endDate)
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách các đơn hàng trong khoảng thời gian đã cho
            var orders = db.HOA_DON.Where(o => o.NgayMua >= startDate && o.NgayMua <= endDate).ToList();

            // Tính tổng doanh thu bằng cách cộng tổng số tiền của các đơn hàng
            decimal totalRevenue = orders.Sum(o => (decimal)(o.TongTien ?? 0));

            return totalRevenue;
        }

        // Hàm tính sản phẩm được mua nhiều nhất
        public string CalculateMostBoughtProduct(DateTime startDate, DateTime endDate)
        {
            // Lấy danh sách tất cả các chi tiết đơn hàng trong khoảng thời gian cụ thể
            var orderDetails = db.HOA_DON_CHI_TIET
                                .Where(od => od.HOA_DON.NgayMua >= startDate && od.HOA_DON.NgayMua <= endDate)
                                .ToList();

            // Tạo một từ điển để lưu trữ số lượng của mỗi sản phẩm
            Dictionary<int, int> productQuantityMap = new Dictionary<int, int>();

            // Tính tổng số lượng của mỗi sản phẩm
            foreach (var orderDetail in orderDetails)
            {
                int productId = orderDetail.MaSP ?? 0;
                int quantity = orderDetail.SoLuongMua ?? 0;

                if (productQuantityMap.ContainsKey(productId))
                {
                    productQuantityMap[productId] += quantity;
                }
                else
                {
                    productQuantityMap.Add(productId, quantity);
                }
            }

            // Xác định sản phẩm có số lượng lớn nhất
            int maxQuantity = productQuantityMap.Values.Max();
            int mostBoughtProductId = productQuantityMap.FirstOrDefault(x => x.Value == maxQuantity).Key;

            // Lấy thông tin của sản phẩm được mua nhiều nhất
            var mostBoughtProduct = db.SAN_PHAM_CHI_TIET.FirstOrDefault(p => p.MaSP == mostBoughtProductId);

            return mostBoughtProduct != null ? mostBoughtProduct.TenSanPham : "Không có sản phẩm được mua";
        }

       

        // Hàm tính tổng số lượng đơn hàng từ trước đến nay
        public int CalculateTotalOrdersFromBeginning()
        {
            // Truy vấn cơ sở dữ liệu để lấy danh sách tất cả các đơn hàng
            var totalOrders = db.HOA_DON.Count();

            return totalOrders;
        }

        

        // Hàm tính số lượng đơn hàng đang chờ xử lý
        public int CountPendingOrders()
        {
            // Lấy danh sách các đơn hàng có trạng thái là "Đang xử lý"
            var pendingOrders = db.HOA_DON.Where(dh => dh.Status == 1).ToList();

            // Đếm số lượng đơn hàng
            int pendingOrdersCount = pendingOrders.Count;

            return pendingOrdersCount;
        }

       
        public int CountCompletedOrders()
        {
            // Lấy danh sách các đơn hàng có trạng thái là "Đã hoàn thành" (ID_TrangThai = 3)
            var completedOrders = db.HOA_DON.Where(dh => dh.Status == 2).ToList();

            // Đếm số lượng đơn hàng
            int completedOrdersCount = completedOrders.Count;

            return completedOrdersCount;
        }
    }

}