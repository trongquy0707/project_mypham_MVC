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
    
    public partial class BINH_LUAN
    {
        public int MaBL { get; set; }
        public Nullable<int> MaSP { get; set; }
        public string BinhLuan { get; set; }
        public string TenNguoiBL { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
    
        public virtual SAN_PHAM_CHI_TIET SAN_PHAM_CHI_TIET { get; set; }
    }
}
