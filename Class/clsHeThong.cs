using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web_Apax.Models
{
    public partial class QuanHuyenModel
    {
        public string QUANHUYEN { get; set; }
        public string TENQUANHUYEN { get; set; }
        public string TINHTHANHPHO { get; set; }
        public string MaQuyUoc { get; set; }
    }

    public class QuyenSuDungModel
    {
        public int QuyenSuDung { get; set; }
        public string TenQuyenSuDung { get; set; }
        public Nullable<int> NhomQuyen { get; set; }
        public string TableLink { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public int Quyen { get; set; }
        public Nullable<bool> IsThuocNhom { get; set; }
    }

    public class NguoiDungModel
    {
        public int NguoiDung { get; set; }
        public string TaiKhoan { get; set; }
        public Nullable<int> NhanSu { get; set; }
        public string HoTen { get; set; }
        public Nullable<DateTime> NgaySinh { get; set; }
        public string DiDong { get; set; }
        public string Email { get; set; }
        public string DiaChiLienHe { get; set; }
        public Nullable<int> ChiNhanh { get; set; }
        public string TenChiNhanh { get; set; }
        public Nullable<bool> TrangThai { get; set; }
        public string TenTrangThai { get; set; }
        public string NguoiTao { get; set; }
        public string NguoiSua { get; set; }
        public Nullable<int> NhomNguoiDung { get; set; }
        public string TenNhomNguoiDung { get; set; }
        public Nullable<DateTime> NgayTao { get; set; }
    }

    public class NhanSuModel
    {
        public string MaNhanSu { get; set; }
        public string TaiKhoanTao { get; set; }
        public string TenTinhThanhPho { get; set; }
        public string TenQuocGia { get; set; }
        public string TenTinhTrangGiaDinh { get; set; }
        public string TENQUANHUYEN { get; set; }
        public string TenChiNhanh { get; set; }
        public Nullable<int> NhanSu { get; set; }
        [Required(ErrorMessage = "<i class=\"ace-icon fa fa-times-circle\"></i>")]
        [EmailAddress(ErrorMessage = "<i class=\"ace-icon fa fa-times-circle\"></i>")]
        public string EMAIL { get; set; }
        public bool ISDEL { get; set; }
        [Required(ErrorMessage = "<i class=\"ace-icon fa fa-times-circle\"></i>")]
        public string HOTEN { get; set; }
        public string GHICHU { get; set; }
        public string AVATAR { get; set; }
        [Required(ErrorMessage = "<i class=\"ace-icon fa fa-times-circle\"></i>")]
        public Nullable<DateTime> NGAYSINH { get; set; }
        public bool ISGIOITINH { get; set; }
        public string DIDONG { get; set; }
        public string DIACHILIENHE { get; set; }
        public string HKTT { get; set; }
        public int QUANHUYEN { get; set; }
        public int TINHTHANHPHO { get; set; }
        public string MaQuocTich { get; set; }
        public string SOCMND { get; set; }
        public Nullable<System.DateTime> NGAYCAPCMND { get; set; }
        public string NOICAPCMND { get; set; }
        public string SOTHICH { get; set; }
        public Nullable<int> TINHTRANGGIADINH { get; set; }
        public string MACHUCVU { get; set; }
        public Nullable<int> CHINHANH { get; set; }
        public Nullable<int> NGUOIDUNG1 { get; set; }
        public Nullable<System.DateTime> NGAY1 { get; set; }
        public Nullable<int> NGUOIDUNG2 { get; set; }
        public Nullable<System.DateTime> NGAY2 { get; set; }
        public string MAY1 { get; set; }
        public string MAY2 { get; set; }
        public string TenGioiTinh { get; set; }
        public string TENCHUCVU { get; set; }
    }
    public class clGioiTinh
    {
        public bool GioiTinh { get; set; }
        public string TenGioiTinh { get; set; }
    }
    public class clsNgonNgu
    {
        public string sKey { get; set; }
        public string sValue { get; set; }
    }
}