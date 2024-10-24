using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WEB_Hieu_Quy.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Tên khách hàng không để trống")]
        public string TenKhachHang { get; set; }
        [Required(ErrorMessage = "Số điện thoại khách hàng không để trống")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Email khách hàng không để trống")]
        public string Email { get; set; }
        public string ThanhPho { get; set; }
        public string Quan_Huyen { get; set; }
        public string Xa_Phuong { get; set; }
        public string Ghi_Chu { get; set; }
        public int Thanh_Toan { get; set; }
        public int Thanh_Toan_VnPay { get; set; }
    }
}