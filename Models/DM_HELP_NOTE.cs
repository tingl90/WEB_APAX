//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_Apax.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DM_HELP_NOTE
    {
        public int ID_HELP_NOTE { get; set; }
        public int ID_HELP { get; set; }
        public string CONTEN_HELP_NOTE { get; set; }
        public byte[] FILES_HELP_NOTE { get; set; }
        public int ID_DTTC { get; set; }
        public System.DateTime NGAYNHAP { get; set; }
        public string TENNGUOINHAP { get; set; }
        public string QUYENNGUOIDUNG { get; set; }
    }
}
