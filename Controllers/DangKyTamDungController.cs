using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DangKyTamDungController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        // GET: DangKyTamDung
        public ActionResult ChiTietTamDung()
        {
            if (TaiKhoan.Trim().ToLower() == "administrator")
                ViewBag.ChucVu = "NV_GIAM_DOC_TT";
            else
                ViewBag.ChucVu = db.DM_DTTC.FirstOrDefault(t => t.TAIKHOAN == TaiKhoan).CHUCVU;
            return View();
        }
        public JsonResult GetDanhSachHocVienCanTamDung()
        {
            try
            {
                var model = db.SP_APAX_TIMKIEM_DANHSACH_DAXEPLOP(0, TaiKhoan,0).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> LoadHocSinhInfo(int? ID)
        {
            try
            {
                var model = await Task.Run(() => db.SP_APAX_TIMKIEM_DANHSACH_DAXEPLOP(ID, TaiKhoan,0).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public JsonResult TamDungHoc(
            int J_HOSOKHACHHANG,
            int J_KEHOACH,
            int? SOBUOICONLAI,
            int? TONGTIEN_DH,
            int J_SANPHAM,
            string DONGIA,
            int? THANHTIEN,
            int A_TH_DUBAO)
        {
            try
            {
                db.TH_DUBAO.Add(new TH_DUBAO()
                {
                    J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                    ID_DTTC = FCVDataProvider.GetUserID(User.Identity.Name),
                    J_KEHOACH = J_KEHOACH,
                    NGAYLAM = DateTime.Now,
                    SOBUOI = SOBUOICONLAI,
                    TONGTIEN_DH = TONGTIEN_DH,
                    ID_LYDO_HV = 102,
                    DULIEUNHOM = true
                });
                db.SaveChanges();
                int a_th_dubao = db.TH_DUBAO.OrderByDescending(t => t.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
                db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
                {
                    J_TH_DUBAO = a_th_dubao,
                    J_SANPHAM = J_SANPHAM,
                    SOLUONG = SOBUOICONLAI == null ? 0 : decimal.Parse(SOBUOICONLAI.ToString()),
                    DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA),
                    THANHTIEN = THANHTIEN
                });
                var model = db.TH_DUBAO.FirstOrDefault(t => t.A_TH_DUBAO == A_TH_DUBAO);
                model.SOBUOI = SOBUOICONLAI;
                model.DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA);
                model.TONGTIEN_DH = TONGTIEN_DH;
                model.ID_LYDO_TD = 239;

                int sc = db.SaveChanges();
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public int TinhBuoiConLai(
           DateTime? NgayChuyen,
           string NgayKetThuc,
           int? KhuVuc,
           int? Buoi1,
           int? Buoi2,
           int? A_DuBao)
        {
            try
            {
                var SoBuoiBaoLuu = db.TH_DUBAO.FirstOrDefault(k => k.A_TH_DUBAO == A_DuBao);
                var model = db.APAX_DemNgayConLaiChuyenPhi(NgayChuyen.Value.ToString("yyyy-MM-dd"), ChuyenDoiChuoiNgayThangNam(NgayKetThuc), Buoi1, Buoi2, KhuVuc).ToList();
                if (model.Count > 0)
                    return model.Count - ((SoBuoiBaoLuu != null && SoBuoiBaoLuu.TONGSOBUOIBAOLUU > 0) ? (int)SoBuoiBaoLuu.TONGSOBUOIBAOLUU : 0);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public int TinhBuoiTamDung(
         DateTime? NgayBatDauTamDung,
         DateTime? NgayKetThucTamDung,
         int? KhuVuc,
         int? Buoi1,
         int? Buoi2,
         int? A_DuBao)
        {
            try
            {
                var SoBuoiBaoLuu = db.TH_DUBAO.FirstOrDefault(k => k.A_TH_DUBAO == A_DuBao);
                var model = db.APAX_DemNgayConLaiChuyenPhi(NgayBatDauTamDung.Value.ToString("yyyy-MM-dd"), NgayKetThucTamDung.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2, KhuVuc).ToList();
                if (model.Count > 0)
                    return model.Count - ((SoBuoiBaoLuu != null && SoBuoiBaoLuu.TONGSOBUOIBAOLUU > 0) ? (int)SoBuoiBaoLuu.TONGSOBUOIBAOLUU : 0);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public JsonResult LoadLichSuTamDung(int HocVien)
        {
            var model = db.SP_APAX_GRIDLICHSUTAMDUNG(HocVien,TaiKhoan).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        private string ChuyenDoiChuoiNgayThangNam(string ngayChon)
        {
            if (ngayChon == "" || ngayChon == null)
                return string.Empty;
            string[] list = ngayChon.ToString().Substring(0, 10).Split('/');
            return list[2] + '-' + list[1] + '-' + list[0];
        }
        public ActionResult LayNgayKetThucHoanChinh(
           DateTime? ngaybd,
           int? SoBuoi,
           int? Buoi1,
           int? Buoi2,
           int? KhuVuc)
        {
            try
            {
                string ngaybatdau = ngaybd.Value.ToString("yyyy-MM-dd");
                var model = db.APAX_HamLayNgayKetThuc(ngaybd.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2, SoBuoi, KhuVuc).ToList();
                return Json(model[0].Date.Value.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(7, JsonRequestBehavior.AllowGet);
            }
        }
    }
}