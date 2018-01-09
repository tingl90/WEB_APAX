using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class HoSoHocVienController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        public ActionResult QuanLyHocVien()
        {
            return View();
        }
        public JsonResult GetHoSoHocVien()
        {
            try
            {
                //Results rs = new Results();
                //var tong = db.SP_APAX_HOSOHOCVIEN_TOTAL(TaiKhoan).ToList();
                //rs.Total = (long)tong[0];
                //rs.Result = db.SP_APAX_HOSOHOCVIEN2(TaiKhoan, take, skip).ToList();
                //var kq = Json(rs, JsonRequestBehavior.AllowGet);
                //kq.MaxJsonLength = int.MaxValue;
                //return kq;
                var model = db.SP_APAX_HOSOHOCVIEN(TaiKhoan).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception ex){
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChiTietHocVien(int? HoSoKhachHang)
        {
            //ViewBag.NguoiDung = User.Identity.Name;
            var model = db.TH_HOSOKHACHHANG.FirstOrDefault(x => x.A_HOSOKHACHHANG == HoSoKhachHang);
            if (model!=null)
            {
                var ten = db.TH_HOSOKHACHHANG.FirstOrDefault(it => it.MAKH.Equals(model.MA_CH));
                ViewBag.TENCH = ten != null ? ten.TENKHACHHANG : "";
            }else
            {
                ViewBag.TENCH = "";
            }
            ViewBag.ChucVuTK = (db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(TaiKhoan))).JD_BOPHAN;
            ViewBag.ChucVuKhac = (db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(TaiKhoan))).CHUCVU;
            return View(model != null ? model : new TH_HOSOKHACHHANG());
        }

        public JsonResult GetNhanSuTheoTrungTamSale(int TrungTam)
        {
            var model = db.APAX_NHANSU_THEOTRUNGTAM(TrungTam, "Sale");
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNhanSuTheoTrungTamCM(int TrungTam)
        {
            var model = db.APAX_NHANSU_THEOTRUNGTAM(TrungTam, "CSO");
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTrungTamTheoTaiKhoan()
        {
            var model = db.SP_APAX_DANHSACHTRUNGTAMTHEOTAIKHOAN(TaiKhoan).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMaHocVienAuto(int? KhachHang)
        {
            if (KhachHang == 0 || KhachHang == null)
            {
                return Json(db.sp_APAX_TaoMaKhachHang("A"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                var model = db.TH_HOSOKHACHHANG.FirstOrDefault(x => x.A_HOSOKHACHHANG == KhachHang).MAKH;
                return Json(model, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult KiemTraCheckTrung(string SoDienThoai,string TenHocVien)
        {
            var model = db.SP_APAX_CHECKTRUNG(SoDienThoai, TenHocVien).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
          
        public JsonResult SaveHocVien(TH_HOSOKHACHHANG khachhang)
        {
            var model = db.TH_HOSOKHACHHANG.FirstOrDefault(x => x.MAKH == khachhang.MAKH);
            if (model != null)
            {
                model.C_makh = khachhang.C_makh;
                model.ma_lms = khachhang.ma_lms;
                model.TENKHACHHANG = khachhang.TENKHACHHANG;
                model.TENTIENGANH = khachhang.TENTIENGANH;
                model.NGAYSINH = khachhang.NGAYSINH;
                model.DIACHI = khachhang.DIACHI;
                model.ID_QUANHUYEN = khachhang.ID_QUANHUYEN;
                model.ID_TINHTHANHPHO = khachhang.ID_TINHTHANHPHO;
                model.ID_GIOITINH = khachhang.ID_GIOITINH;
                model.DIENTHOAI1 = khachhang.DIENTHOAI1;
                model.EMAIL = khachhang.EMAIL;
                model.MA_CH = khachhang.MA_CH;
                model.TENME = khachhang.TENME;
                model.DD_ME = khachhang.DD_ME;
                model.EMAIL_ME = khachhang.EMAIL_ME;
                model.TENBO = khachhang.TENBO;
                model.DD_BO = khachhang.DD_BO;
                model.EMAIL_BO = khachhang.EMAIL_BO;
                model.SOCMTND = khachhang.SOCMTND;
                model.ID_LOAIHOSOKH = khachhang.ID_LOAIHOSOKH;
                model.J_SALE_MAN = khachhang.J_SALE_MAN;
                model.J_CSO = khachhang.J_CSO;
                model.ID_TRUNGTAM = khachhang.ID_TRUNGTAM;
                model.MATRUNGTAM = (db.DM_TRUNGTAM.FirstOrDefault(it => it.ID_TRUNGTAM == khachhang.ID_TRUNGTAM)).MA_DVCS;
                db.SaveChanges();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var hosokhachhang = new TH_HOSOKHACHHANG();
                hosokhachhang.MAKH = khachhang.MAKH;
                hosokhachhang.C_makh = khachhang.C_makh;
                hosokhachhang.ma_lms = khachhang.ma_lms;
                hosokhachhang.TENKHACHHANG = khachhang.TENKHACHHANG;
                hosokhachhang.TENTIENGANH = khachhang.TENTIENGANH;
                hosokhachhang.NGAYSINH = khachhang.NGAYSINH;
                hosokhachhang.DIACHI = khachhang.DIACHI;
                hosokhachhang.ID_QUANHUYEN = khachhang.ID_QUANHUYEN;
                hosokhachhang.ID_TINHTHANHPHO = khachhang.ID_TINHTHANHPHO;
                hosokhachhang.ID_GIOITINH = khachhang.ID_GIOITINH;
                hosokhachhang.DIENTHOAI1 = khachhang.DIENTHOAI1;
                hosokhachhang.EMAIL = khachhang.EMAIL;
                hosokhachhang.MA_CH = khachhang.MA_CH;
                hosokhachhang.TENME = khachhang.TENME;
                hosokhachhang.DD_ME = khachhang.DD_ME;
                hosokhachhang.EMAIL_ME = khachhang.EMAIL_ME;
                hosokhachhang.TENBO = khachhang.TENBO;
                hosokhachhang.DD_BO = khachhang.DD_BO;
                hosokhachhang.EMAIL_BO = khachhang.EMAIL_BO;
                hosokhachhang.SOCMTND = khachhang.SOCMTND;
                hosokhachhang.ID_LOAIHOSOKH = khachhang.ID_LOAIHOSOKH;
                hosokhachhang.J_SALE_MAN = khachhang.J_SALE_MAN;
                hosokhachhang.J_CSO = khachhang.J_CSO;
                hosokhachhang.ID_TRUNGTAM = khachhang.ID_TRUNGTAM;
                hosokhachhang.MATRUNGTAM = (db.DM_TRUNGTAM.FirstOrDefault(it => it.ID_TRUNGTAM == khachhang.ID_TRUNGTAM)).MA_DVCS;
                db.TH_HOSOKHACHHANG.Add(hosokhachhang);
                db.SaveChanges();
                var A_HOSOKHACHHANG = db.TH_HOSOKHACHHANG.OrderByDescending(x => x.A_HOSOKHACHHANG).FirstOrDefault().A_HOSOKHACHHANG;
                return Json(A_HOSOKHACHHANG, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetECTheoTaiKhoan(string tk)
        {
            tk = !String.IsNullOrEmpty(tk) && tk != "" ? (db.DM_DTTC.FirstOrDefault(it => it.MADTTC.Equals(tk))).TAIKHOAN : TaiKhoan;
            var model = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(tk) && (it.JD_BOPHAN == 180 || it.JD_BOPHAN == 182));
            return Json(model != null ? model : null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCMTheoTaiKhoan(string tk)
        {
            tk = !String.IsNullOrEmpty(tk) && tk != "" ? (db.DM_DTTC.FirstOrDefault(it => it.MADTTC.Equals(tk))).TAIKHOAN : TaiKhoan;
            var model = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(tk) && (it.JD_BOPHAN == 183 || it.JD_BOPHAN==185));
            return Json(model != null ? model : null, JsonRequestBehavior.AllowGet);
        }
        public string GetTenAnhEmHocCung(string ma)
        {
            var model = db.TH_HOSOKHACHHANG.FirstOrDefault(it => it.MAKH.Equals(ma));
            return model != null ? model.TENKHACHHANG : "";
        }
        public JsonResult LoadLichSuXepLop(int HocVien)
        {
            return Json(db.SP_APAX_LICHSUXEPLOP(HocVien).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadLichSuTamDung(int HocVien)
        {
            return Json(db.TH_DUBAO.Where(it=>it.J_HOSOKHACHHANG==HocVien && it.ID_LYDO_HV==102).ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}