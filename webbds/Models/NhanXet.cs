//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace webbds.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NhanXet
    {
        public int MaNhanXet { get; set; }
        public Nullable<int> MaBatDongSan { get; set; }
        public Nullable<int> MaNguoiDung { get; set; }
        public Nullable<int> DanhGia { get; set; }
        public string BinhLuan { get; set; }
    
        public virtual BatDongSan BatDongSan { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
