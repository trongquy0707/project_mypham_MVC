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
    
    public partial class HOA_DON_CHI_TIET
    {
        public int MaHDCT { get; set; }
        public Nullable<int> MaHD { get; set; }
        public Nullable<int> MaSP { get; set; }
        public Nullable<int> SoLuongMua { get; set; }
        public Nullable<double> GiaBan { get; set; }
    
        public virtual HOA_DON HOA_DON { get; set; }
        public virtual SAN_PHAM_CHI_TIET SAN_PHAM_CHI_TIET { get; set; }
    }
}
