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
    
    public partial class DM_BOPHAN
    {
        public int ID_BOPHAN { get; set; }
        public string TENBOPHAN { get; set; }
        public string QUYENNGUOIDUNG { get; set; }
        public Nullable<int> ID_TRUNGTAM { get; set; }
        public string ma_crm { get; set; }
        public Nullable<int> ma_lms { get; set; }
        public string Status { get; set; }
    }
}