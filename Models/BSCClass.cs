using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Apax.Models
{
    public class BSCClass
    {
    }
    public partial class BangCongThang
    {
        public string MAKH { get; set; }
        public string TENKHACHHANG { get; set; }
        public string TENQUOCGIA { get; set; }
        public string TENCHINHANH { get; set; }
        public string TENPHONGBAN { get; set; }
        public int? NGAY_1 { get; set; }
        public int? NGAY_2 { get; set; }
        public int? NGAY_3 { get; set; }
        public int? NGAY_4 { get; set; }
        public int? NGAY_5 { get; set; }
        public int? NGAY_6 { get; set; }
        public int? NGAY_7 { get; set; }
        public int? NGAY_8 { get; set; }
        public int? NGAY_9 { get; set; }
        public int? NGAY_10 { get; set; }
        public int? NGAY_11 { get; set; }
        public int? NGAY_12 { get; set; }
        public int? NGAY_13 { get; set; }
        public int? NGAY_14 { get; set; }
        public int? NGAY_15 { get; set; }
        public int? NGAY_16 { get; set; }
        public int? NGAY_17 { get; set; }
        public int? NGAY_18 { get; set; }
        public int? NGAY_19 { get; set; }
        public int? NGAY_20 { get; set; }
        public int? NGAY_21 { get; set; }
        public int? NGAY_22 { get; set; }
        public int? NGAY_23 { get; set; }
        public int? NGAY_24 { get; set; }
        public int? NGAY_25 { get; set; }
        public int? NGAY_26 { get; set; }
        public int? NGAY_27 { get; set; }
        public int? NGAY_28 { get; set; }
        public int? NGAY_29 { get; set; }
        public int? NGAY_30{ get; set; }
        public int? NGAY_31 { get; set; }
        public int? TONG_MUON { get; set; }
        public int? TONG_PHAT { get; set; }
        public string Tienphat { get; set; }

    }
    public partial class HOCSINHLOPHOC
    {
        public int A_KEHOACH_KHACHHANG { get; set; }
    }
    public partial class Q_SendMail
    {
        public string SMTPServer { get; set; }
        public string Email { get; set; }
        public string MatKhau { get; set; }
        public int Port { get; set; }
    }
    public partial class DanhSachChoDuyet
    {
        public int? A_HOSOKHACHHANG { get; set; }
        public int? A_TH_DUBAO { get; set; }
        public string MAKH { get; set; }
        public string TENKHACHHANG { get; set; }
        public string Email { get; set; }
        public string WEBSITE { get; set; }
        public string DIDONG { get; set; }
        public DateTime? NGAYLAM { get; set; }
        public string GHICHU { get; set; }
        public string LOAITHUCHIEN { get; set; }
        public decimal? SONGAYNGHI { get; set; }
        public string TRANGTHAI { get; set; }
        public string TRANGTHAIDUYET { get; set; }
        public string TENNGUOIDUYET { get; set; }
        public DateTime? NGAYDUYET { get; set; }
        public string YKIENBOSUNG { get; set; }
    }
    public partial class ThongTinDuyet
    {
        public int A_DUYET { get; set; }
        public int DUNGTRAN { get; set; }
        public string NGAYDUYET { get; set; }
        public string NGUOIDUYET { get; set; }
        public int? ID_DUYET { get; set; }
        public string YKIENDUYET { get; set; }
        public bool? ChoSua { get; set; }
    }
    public partial class DanhSachNhanVien
    {
        public string BUTTON { get; set; }
        public int? A_HOSOKHACHHANG { get; set; }
        public string MAKH { get; set; }
        public string TENKHACHHANG { get; set; }
        public string TENQUOCGIA { get; set; }
        public string TENCHINHANH { get; set; }
        public string TENPHONGBAN { get; set; }
        public string TENBOPHAN { get; set; }
        public string TENCHUCVU { get; set; }
        public string NGUOIQUANLY { get; set; }
        public string Email { get; set; }
        public string WEBSITE { get; set; }
        public string DIDONG { get; set; }
        public DateTime? NGAYVAOCTY { get; set; }
        public DateTime? NGAYKYHDCT { get; set; }
    }
    public partial class MangSoDoToChuc
    {
        public string ID { get; set; }
        public string Head_ID { get; set; }
        public string Title { get; set; }
        public int IDChiNhanh { get; set;}
    }
    public partial class SoDoChiTietToChucChiNhanh
    {
        public string ID { get; set; }
        public string Head_ID { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string ChucVuFax { get; set; }
        public string MailDiaChi { get; set; }
        public string HinhAnh { get; set; }
    }
}