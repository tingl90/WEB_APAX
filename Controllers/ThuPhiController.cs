using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class ThuPhiController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        public ActionResult ThuHocPhi()
        {
            ViewBag.NguoiThuHocPhi = User.Identity.Name;
            return View();
        }

        public JsonResult LoadDanhSachHocSinhChuaXepLop(string MaHocVien)
        {
            var model = db.SP_APAX_TIMKIEM_DANGKYHOC_CHUAXEPLOP(MaHocVien,0, TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult GetDanhSachHocSinhChuaXepLop(int HocVien)
        {
            var model = db.SP_APAX_TIMKIEM_DANGKYHOC_CHUAXEPLOP("", HocVien, TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult SaveThuHocPhi(int J_HOSOKHACHHANG, int J_TH_HOPDONG, string NgayThuTien, string SoTienThu, string NguoiThuTien,
            int HinhThucThu,int LoaiThuTien,string GhiChu,string EC, string CM, string ECLEADER, string CMLEADER, string GDTT, string GDV)
        {
            try
            {
                var tn = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(NguoiThuTien));
                if (tn != null && tn.JD_BOPHAN == 184)
                {
                    var th_thuhien = new TH_THUTIEN();
                    th_thuhien.J_HOSOKHACHHANG = J_HOSOKHACHHANG;
                    th_thuhien.J_TH_HOPDONG = J_TH_HOPDONG;
                    th_thuhien.NGAYTHUTIEN = DateTime.Parse(NgayThuTien);
                    th_thuhien.SOTIENTHU = decimal.Parse(SoTienThu.Replace(",", ""));
                    th_thuhien.LOAI = 1;
                    th_thuhien.ID_LOAIPHIEUTHU = HinhThucThu;
                    th_thuhien.ID_LOAITHUTIEN = LoaiThuTien;
                    th_thuhien.GHICHUTHUTIEN = GhiChu;
                    th_thuhien.ID_DTTC = tn.ID_DTTC;
                    th_thuhien.TK_EC = EC.Trim();
                    th_thuhien.TK_EC_LEADER = ECLEADER.Trim();
                    th_thuhien.TK_CM = CM.Trim();
                    th_thuhien.TK_CM_LEADER = CMLEADER.Trim();
                    th_thuhien.TK_GDTT = GDTT.Trim();
                    th_thuhien.TK_GDV = GDV.Trim();
                    db.TH_THUTIEN.Add(th_thuhien);
                    return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LichSuThuPhi(int KhachHang, int HopDong,int loai)
        {
            var model = db.SP_APAX_LICHSUTHUTIEN(KhachHang, HopDong,loai).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChiTietLichSu(int KhachHang, int HopDong, int loai,int LichSu)
        {
            var model = db.SP_APAX_LICHSUTHUTIEN(KhachHang, HopDong, loai).Where(it => it.A_TH_THUTIEN == LichSu).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult XoaThuTien(string A_TH_THUTIEN)
        {
            var tt = A_TH_THUTIEN.Split(',');
            foreach (var it in tt)
            {
                int thutien = int.Parse(it);
                db.Database.ExecuteSqlCommand("DELETE dbo.TH_THUTIEN WHERE A_TH_THUTIEN="+thutien);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        #region Người thu phí

        public JsonResult DM_NguoiThuPhi()
        {
            var model = db.DM_DTTC.Where(it=>it.JD_BOPHAN==184).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_CB_HinhThucThu()
        {
            var model = db.DM_LOAIPHIEUTHU.OrderBy(it=>it.ID_LOAIPHIEUTHU).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Get_CB_LoaiThuTien()
        {
            var model = db.DM_LOAITHUTIEN.ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}