//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WEB_Hieu_Quy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HOA_DON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HOA_DON()
        {
            this.HOA_DON_CHI_TIET = new HashSet<HOA_DON_CHI_TIET>();
        }
    
        public int MaHD { get; set; }
        public DateTime NgayMua { get; set; }
        public Nullable<double> TongTien { get; set; }
        public string TenKhachHang { get; set; }
        public string Code { get; set; }
        public string ThanhPho { get; set; }
        public string Quan_Huyen { get; set; }
        public string Xa_Phuong { get; set; }
        public string Ghi_Chu { get; set; }
        public string Email { get; set; }
        public Nullable<int> Thanh_Toan { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string SDT { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> MaVC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOA_DON_CHI_TIET> HOA_DON_CHI_TIET { get; set; }
        public virtual VOUCHER VOUCHER { get; set; }
    }
}
