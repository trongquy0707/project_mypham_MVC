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
    
    public partial class VOUCHER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VOUCHER()
        {
            this.HOA_DON = new HashSet<HOA_DON>();
        }
    
        public int MaVC { get; set; }
        public Nullable<decimal> ToiThieu { get; set; }
        public Nullable<decimal> GiamGia { get; set; }
        public string TenVoucher { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HOA_DON> HOA_DON { get; set; }
    }
}
