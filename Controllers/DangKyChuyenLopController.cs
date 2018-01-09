using System;
using System.Linq;
using System.Web.Mvc;
using Web_Apax.Models;
namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DangKyChuyenLopController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        public ActionResult ChiTietChuyenLop()
        {
            ViewBag.TrungTam = db.Database.SqlQuery<int>(@"SELECT ID_TRUNGTAM FROM dbo.DM_TRUNGTAM WHERE MA_DVCS = (SELECT MATRUNGTAM FROM dbo.DM_DTTC WHERE TAIKHOAN = '" + TaiKhoan + "')").FirstOrDefault();
            return View();
        }
        public JsonResult GetDanhSachHocSinhDaXepLop()
        {
            var model = db.SP_APAX_DANHSACHDAXEPLOP("", TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult LoadLichSuChuyenLop(int HocVien)
        {
            var model = db.SP_APAX_GRIDLICHSUCHUYENTRUNGTAM(HocVien, TaiKhoan).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChiTietLichSu(int? DUBAO)
        {
            var model = db.SP_APAX_CHITIETLICHSUCHUYENLOP(DUBAO).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadDanhSachHocSinhDaXepLop(int HocVien, int LopHoc)
        {
            var model = db.SP_APAX_GETDANHSACHDAXEPLOPCANCHUYEN(HocVien, LopHoc).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachLopHoc(int? TrungTam, int? LopCu)
        {
            var model = db.SP_APAX_DANHSACHLOP(TrungTam, LopCu , TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult LoadDanhSachLop(int LopHoc)
        {
            var model = db.SP_APAX_GETDANHSACHLOP(LopHoc).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachTrungTam(string TenTrungTam)
        {
            var model = db.SP_APAX_DANHSACHTRUNGTAM(TenTrungTam).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult LoadDanhSachTrungTam(int TrungTam)
        {
            var model = db.SP_APAX_GETDANHSACHTRUNGTAM(TrungTam).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChuyenTrungTam(
            int? A_THDUBAO,
            int J_HOSOKHACHHANG,
            int LOPCU,
            DateTime? NGAYGIAOHANG_LOPCU,
            DateTime? NGAYTHANHTOAN_LOPCU,
            decimal? THANHTIEN_LOPCU,
            int SOBUOI,
            decimal? DONGIA_LOPCU,
            int? ID_BOPHAN,
            int? TRUNGTAMDI,
            int? TRUNGTAMDEN,
            int? CHIETKHAU_LOPCU,
            string GHICHU,
            int SANPHAM_LOPCU)
        {
            //Hủy bỏ thông tin học viên trước đó
            int sc = 0;
            var nguoidung = db.DM_DTTC.FirstOrDefault(t => t.TAIKHOAN == TaiKhoan).ID_DTTC;
            var modelLopCu = db.TH_DUBAO.FirstOrDefault(x => x.A_TH_DUBAO == A_THDUBAO);
            if (modelLopCu != null)
            {
                modelLopCu.NGAYTHANHTOAN = DateTime.Now;
                modelLopCu.SOBUOI = 0;
                modelLopCu.ID_LYDO_TD = 240;
                modelLopCu.ID_DTTC = nguoidung;
            }
            db.SaveChanges();

            //Thiết lập thông tin lớp cũ
            db.TH_DUBAO.Add(new TH_DUBAO()
            {
                J_KEHOACH = LOPCU,// lớp cũ
                J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                NGAYLAM = DateTime.Now,
                NGAYGIAOHANG = NGAYGIAOHANG_LOPCU,
                NGAYTHANHTOAN = NGAYTHANHTOAN_LOPCU,
                TONGTIEN_DH = THANHTIEN_LOPCU,
                SOBUOI = SOBUOI,
                DONGIA = DONGIA_LOPCU,
                ID_LYDO_HV = 1104,  // Để nghị chuyển trung tâm
                ID_LYDO_TD = 241,  // Đề nghị - chờ duyệt
                ID_TRUNGTAM_DI = TRUNGTAMDI,
                ID_TRUNGTAM_DEN = TRUNGTAMDEN,
                GHICHU = !string.IsNullOrEmpty(GHICHU) ? GHICHU : "",
                ID_BOPHAN = TRUNGTAMDEN,
                A_THUCHIEN = A_THDUBAO,
                ID_DTTC = nguoidung
            });
            db.SaveChanges();
            var a_th_dubao = db.TH_DUBAO.OrderByDescending(x => x.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
            db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
            {
                J_TH_DUBAO = a_th_dubao,
                J_SANPHAM = SANPHAM_LOPCU,
                SOLUONG = SOBUOI,
                DONGIA = DONGIA_LOPCU,
                CHIETKHAU = CHIETKHAU_LOPCU,
                THANHTIEN = THANHTIEN_LOPCU
            });
            sc += db.SaveChanges();
            return Json(sc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChuyenLopCungTrungTam(
            int? A_THDUBAO,
            int J_HOSOKHACHHANG,
            int? LOPCU,
            int? LOPMOI,
            DateTime? NGAYGIAOHANG_LOPCU,
            DateTime? NGAYTHANHTOAN_LOPCU,
            decimal? THANHTIEN_LOPCU,
            decimal? THANHTIEN_LOPMOI,
            decimal? DONGIA_LOPCU,
            decimal? DONGIA_LOPMOI,
            int SOBUOI,
            int? TRUNGTAM_LOPCU,
            int? TRUNGTAM_LOPMOI,
            int? CHIETKHAU_LOPCU,
            int? CHIETKHAU_LOPMOI,
            int SANPHAM_LOPCU,
            int SANPHAM_LOPMOI,
            DateTime? NGAYGIAOHANG_LOPMOI,
            DateTime? NGAYTHANHTOAN_LOPMOI,
            string GHICHU)
        {
            int sc = 0;

            //Hủy thông tin lớp cũ
            var nguoidung = db.DM_DTTC.FirstOrDefault(t => t.TAIKHOAN == TaiKhoan).ID_DTTC;
            var model2 = db.TH_DUBAO.FirstOrDefault(x => x.J_HOSOKHACHHANG == J_HOSOKHACHHANG);
            if (model2 != null)
            {
                model2.NGAYTHANHTOAN = DateTime.Now;
                model2.SOBUOI = 0;
                model2.ID_LYDO_TD = 240;
                model2.ID_DTTC = nguoidung;
            }
            db.SaveChanges();
            var a_th_hopdong = db.TH_HOPDONG.FirstOrDefault(t => t.J_HOSOKHACHHANG == J_HOSOKHACHHANG).A_TH_HOPDONG;

            //Thiết lập thông tin lớp cũ
            db.TH_DUBAO.Add(new TH_DUBAO()
            {
                J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                J_KEHOACH = LOPCU,
                J_TH_HOPDONG = a_th_hopdong,
                NGAYLAM = DateTime.Now,
                NGAYGIAOHANG = NGAYGIAOHANG_LOPCU,
                NGAYTHANHTOAN = NGAYTHANHTOAN_LOPCU,
                TONGTIEN_DH = THANHTIEN_LOPCU,
                SOBUOI = SOBUOI,
                DONGIA = DONGIA_LOPCU,
                ID_LYDO_HV = 101,
                ID_LYDO_TD = 241, 
                ID_TRUNGTAM_DI = TRUNGTAM_LOPCU,
                ID_TRUNGTAM_DEN = TRUNGTAM_LOPMOI,
                ID_BOPHAN = TRUNGTAM_LOPCU,
                GHICHU = !string.IsNullOrEmpty(GHICHU) ? GHICHU : "",
                ID_DTTC = nguoidung
            });
            db.SaveChanges();
            var a_th_duabao_lopcu = db.TH_DUBAO.OrderByDescending(x => x.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
            db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
            {
                J_TH_DUBAO = a_th_duabao_lopcu,
                J_SANPHAM = SANPHAM_LOPCU,
                SOLUONG = SOBUOI,
                DONGIA = DONGIA_LOPCU,
                CHIETKHAU = CHIETKHAU_LOPCU == null ? 0 : CHIETKHAU_LOPCU,
                THANHTIEN = THANHTIEN_LOPCU,
                DATENOTE1 = NGAYGIAOHANG_LOPCU != null ? NGAYGIAOHANG_LOPCU : DateTime.Now,
                DATENOTE2 = NGAYTHANHTOAN_LOPCU != null ? NGAYTHANHTOAN_LOPCU : DateTime.Now
            });
            db.SaveChanges();

            //Thiết lập thông tin lớp moi
            db.TH_DUBAO.Add(new TH_DUBAO()
            {
                J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                J_KEHOACH = LOPMOI,
                J_TH_HOPDONG = a_th_hopdong,
                SOBUOI = SOBUOI,
                NGAYLAM = DateTime.Now,
                NGAYGIAOHANG = NGAYGIAOHANG_LOPMOI,
                NGAYTHANHTOAN = NGAYTHANHTOAN_LOPMOI,
                TONGTIEN_DH = THANHTIEN_LOPMOI,
                DONGIA = DONGIA_LOPMOI,
                ID_LYDO_HV = 2103,  //Đề nghị chuyển lớp
                ID_LYDO_TD = 241,
                ID_TRUNGTAM_DI = TRUNGTAM_LOPCU,
                ID_TRUNGTAM_DEN = TRUNGTAM_LOPMOI,
                ID_BOPHAN = TRUNGTAM_LOPMOI,
                GHICHU = !string.IsNullOrEmpty(GHICHU) ? GHICHU : "",
                A_THUCHIEN = a_th_duabao_lopcu,
                ID_DTTC = nguoidung
            });
            db.SaveChanges();
            var a_th_duabao_lopmoi = db.TH_DUBAO.OrderByDescending(x => x.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
            db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
            {
                J_TH_DUBAO = a_th_duabao_lopmoi,
                J_SANPHAM = SANPHAM_LOPMOI,
                SOLUONG = SOBUOI,
                DONGIA = DONGIA_LOPMOI,
                CHIETKHAU = CHIETKHAU_LOPMOI == null ? 0 : CHIETKHAU_LOPMOI,
                THANHTIEN = THANHTIEN_LOPMOI,
                DATENOTE1 = NGAYGIAOHANG_LOPMOI != null ? NGAYGIAOHANG_LOPMOI : DateTime.Now,
                DATENOTE2 = NGAYTHANHTOAN_LOPMOI != null ? NGAYTHANHTOAN_LOPMOI : DateTime.Now
            });
            sc += db.SaveChanges();
            return Json(sc, JsonRequestBehavior.AllowGet);
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
        public ActionResult LayNGayKetThucHoanChinh(
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
        private string ChuyenDoiChuoiNgayThangNam(string ngayChon)
        {
            if (ngayChon == "" || ngayChon == null)
                return string.Empty;
            string[] list = ngayChon.ToString().Substring(0, 10).Split('/');
            return list[2] + '-' + list[1] + '-' + list[0];
        }
        public int TinhBuoiConLaiKhac(DateTime? NgayBatDau, DateTime? NgayChuyen, DateTime? NgayKetThuc, int? KhuVuc, int? HocVien, int? LopHoc, int? Buoi1, int? Buoi2, int? A_DuBao)
        {
            try
            {
                var SoBuoiBaoLuu = db.TH_DUBAO.FirstOrDefault(k => k.A_TH_DUBAO == A_DuBao);
                var model = db.APAX_DemNgayConLaiChuyenPhi(NgayChuyen.Value.ToString("yyyy-MM-dd"), NgayKetThuc.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2, KhuVuc).ToList();
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

        #region dũng
        public JsonResult LoadNguoiDung(int? DuBao)
        {
            var a_th_dubao = db.TH_DUBAO.FirstOrDefault(t => t.A_THUCHIEN == DuBao).A_TH_DUBAO;
            var NguoiDung = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(User.Identity.Name));
            try
            {
                var model = db.APAX_DANHSACHLICHSUDUYET(a_th_dubao).FirstOrDefault();
                if (model != null)
                {
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(NguoiDung.TENDTTC, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(NguoiDung.TENDTTC, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveDuyetChuyenLop(int? A_THDUBAO, int? IDDuyet, string NoiDung)
        {
            int sc = 0;
            try
            {
                var a_th_dubao = db.TH_DUBAO.FirstOrDefault(t => t.A_THUCHIEN == A_THDUBAO).A_TH_DUBAO;
                TH_DUYETTHUCHIEN New = new TH_DUYETTHUCHIEN()
                {
                    
                    J_TH_DUBAO = a_th_dubao,
                    ID_DTTC = clsFunctions.GetUserID(),
                    NGAYDUYET = DateTime.Now,
                    ID_DUYET = IDDuyet,
                    YKIENBOSUNG = NoiDung
                };
                db.Set<TH_DUYETTHUCHIEN>().Add(New);
                sc += db.SaveChanges();
                if (sc > 0)
                {
                    sc += db.Database.ExecuteSqlCommand("Update TH_DUBAO set ID_LYDO_TD = 242 where A_TH_DUBAO =" + a_th_dubao);
                }
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDanhSachLichSuDuyet(int? DuBao)
        {
            var a_th_dubao = db.TH_DUBAO.FirstOrDefault(t => t.A_THUCHIEN == DuBao).A_TH_DUBAO;
            var model = db.APAX_DANHSACHLICHSUDUYET(a_th_dubao).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        #endregion
    }
}