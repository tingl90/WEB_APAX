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
    public class TiepNhanChuyenLopController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        // GET: TiepNhanChuyenLop
        public ActionResult DanhSachTiepNhan()
        {
            var NguoiDung = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(User.Identity.Name));
            ViewBag.ChucVuTK = (db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(User.Identity.Name))).JD_BOPHAN;
            if (NguoiDung != null)
                ViewBag.TenNguoiDung = NguoiDung.TENDTTC;
            return View();
        }
        
        public JsonResult GetDanhSachChuyenLop()
        {
            var nguoidung = User.Identity.Name;
            var model = db.SP_DanhSachDeNghiChuyenLop(nguoidung).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult ChiTietTiepNhan(int? HocVien)
        {
            ViewBag.HocVien = HocVien.ToString();
            return View();
        }

        public JsonResult GETDANHSACHCHUYENLOPTUTTKHAC(int? DuBao)
        {
            var nguoidung = User.Identity.Name;
            var model = db.SP_APAX_GETDANHSACHCHUYENLOPTUTTKHAC(nguoidung,DuBao).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachLichSuDuyet(int? DuBao)
        {
            var model = db.APAX_DANHSACHLICHSUDUYETTIEPNHANCHUYENLOP(DuBao).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult LoadNguoiDung(int? DuBao)
        {
            var NguoiDung = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(User.Identity.Name));
            try
            {
                var model = db.APAX_DANHSACHLICHSUDUYETTIEPNHANCHUYENLOP(DuBao).FirstOrDefault();
                if(model!=null)
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
        public JsonResult CapNhatDangKyChuyenLopCungTrungTam(
            int J_HOSOKHACHHANG,
            int? J_KEHOACH, 
            DateTime? NGAYGIAOHANG, 
            decimal? TONGTIEN_DH, 
            int? SOBUOI,
            decimal? DONGIA,
            int? ID_BOPHAN, 
            string GHICHU, 
            int? J_SANPHAM, 
            DateTime? NGAYDUKIENHOC,
            int? J_HOPDONG,
            string TK_EC,
            string TK_EC_LEADER,
            string TK_CM,
            string TK_CM_LEADER,
            string TK_GDTT,
            string TK_GDV,
            int? ID_TRUNGTAM_DI,
            int? ID_TRUNGTAM_DEN,
            int? A_DuBao
            )
        {
            int sc = 0;
            var thdubao = new TH_DUBAO();
            thdubao.J_KEHOACH = J_KEHOACH;
            thdubao.J_TH_HOPDONG = J_HOPDONG;
            thdubao.SOBUOI = SOBUOI;
            thdubao.NGAYLAM = DateTime.Now;
            thdubao.NGAYGIAOHANG = NGAYDUKIENHOC;
            thdubao.NGAYTHANHTOAN = NGAYGIAOHANG != null ? NGAYGIAOHANG : null;
            thdubao.ID_LYDO_HV = 101;
            thdubao.ID_LYDO_TD = 241;
            thdubao.ID_DTTC = FCVDataProvider.GetUserID(User.Identity.Name);
            thdubao.TK_EC = TK_EC;
            thdubao.TK_EC_LEADER = TK_EC_LEADER;
            thdubao.TK_CM = TK_CM;
            thdubao.TK_CM_LEADER = TK_CM_LEADER;
            thdubao.TK_GDTT = TK_GDTT;
            thdubao.TK_GDV = TK_GDV;
            thdubao.ID_TRUNGTAM_DI = ID_TRUNGTAM_DI;
            thdubao.ID_TRUNGTAM_DEN = ID_TRUNGTAM_DEN;
            thdubao.J_HOSOKHACHHANG = J_HOSOKHACHHANG;
            thdubao.DULIEUNHOM = true;
            thdubao.TONGTIEN_DH = TONGTIEN_DH;
            thdubao.DONGIA = DONGIA;
            thdubao.ID_BOPHAN = ID_BOPHAN;
            thdubao.GHICHU = !string.IsNullOrEmpty(GHICHU) ? GHICHU : "";
            thdubao.A_THUCHIEN = A_DuBao;
            db.TH_DUBAO.Add(thdubao);
            sc+= db.SaveChanges();
            if(sc>0)
            {
                int model = db.TH_DUBAO.OrderByDescending(x => x.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
                var thdubaosanpham = new TH_DUBAO_SANPHAM();
                thdubaosanpham.J_TH_DUBAO = model;
                thdubaosanpham.J_SANPHAM = J_SANPHAM > 0 ? (int)J_SANPHAM : 0;
                thdubaosanpham.SOLUONG = SOBUOI > 0 ? (int)SOBUOI : 0;
                thdubaosanpham.DONGIA = DONGIA;
                thdubaosanpham.CHIETKHAU = 0;
                thdubaosanpham.THANHTIEN = TONGTIEN_DH;
                //thdubaosanpham.DATENOTE1 = NGAYDUKIENHOC != null ? NGAYDUKIENHOC : DateTime.Now;
                //thdubaosanpham.DATENOTE2 = NGAYGIAOHANG != null ? NGAYGIAOHANG : DateTime.Now;
                db.TH_DUBAO_SANPHAM.Add(thdubaosanpham);
                sc += db.SaveChanges();
            }
            if (sc > 0)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDuyetChuyenTrungTam(int? A_THDUBAO,int? IDDuyet,string NoiDung)
        {
            int sc = 0;
            try
            {
                TH_DUYETTHUCHIEN New = new TH_DUYETTHUCHIEN()
                {
                    J_TH_DUBAO = A_THDUBAO,
                    ID_DTTC = clsFunctions.GetUserID(),
                    NGAYDUYET = DateTime.Now,
                    ID_DUYET = IDDuyet,
                    YKIENBOSUNG = NoiDung
                };
                db.Set<TH_DUYETTHUCHIEN>().Add(New);
                sc += db.SaveChanges();
                if(sc>0)
                {
                    sc += db.Database.ExecuteSqlCommand("Update TH_DUBAO set ID_LYDO_TD = 1241 where A_TH_DUBAO =" + A_THDUBAO);
                }
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadThongTinHocSinh(int? HocSinh)
        {
            try
            {
                var TrungTam= Task.Run(() => db.APAX_GetTrungTamBayUserName(User.Identity.Name).FirstOrDefault()).Result;
                var model = db.TH_HOSOKHACHHANG.FirstOrDefault(it => it.A_HOSOKHACHHANG == HocSinh);
                model.ID_TRUNGTAM = TrungTam.TrungTam;
                model.MATRUNGTAM = TrungTam.MA_DVCS;
                if (model != null)
                {
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveHocVien(TH_HOSOKHACHHANG khachhang)
        {
            int sc = 0;
            var model = db.TH_HOSOKHACHHANG.FirstOrDefault(x => x.A_HOSOKHACHHANG == khachhang.A_HOSOKHACHHANG);
            if (model != null)
            {
                model.J_SALE_MAN = khachhang.J_SALE_MAN;
                model.J_CSO = khachhang.J_CSO;
                model.ID_TRUNGTAM = khachhang.ID_TRUNGTAM;
                model.MATRUNGTAM = khachhang.MATRUNGTAM;
                sc= db.SaveChanges();
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDanhSachLopHoc(int? TrungTam, int? LopCu)
        {
            var model = db.SP_APAX_DANHSACHLOPCHUYENTT(User.Identity.Name).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult LoadDanhSachLop(int LopHoc)
        {
            var model = db.SP_APAX_GETDANHSACHLOP(LopHoc).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #region Danh SachDe Nghi Xep Lop tu Trung Tam Khac
        public JsonResult GetDanhSachDeNghiXepLopTuTTKhac()
        {
            var nguoidung = User.Identity.Name;
            var model = db.SP_DanhSachDeNghiTiepNhan(nguoidung).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult GETCHITIETXEPLOP(int? DuBao)
        {
            var nguoidung = User.Identity.Name;
            var model = db.SP_APAX_GETCHITIETXEPLOP(nguoidung.Trim(), DuBao).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveDuyetXepLop(int? A_THDUBAO, int? IDDuyet, string NoiDung)
        {
            int sc = 0;
            try
            {
                TH_DUYETTHUCHIEN New = new TH_DUYETTHUCHIEN()
                {
                    J_TH_DUBAO = A_THDUBAO,
                    ID_DTTC = clsFunctions.GetUserID(),
                    NGAYDUYET = DateTime.Now,
                    ID_DUYET = IDDuyet,
                    YKIENBOSUNG = NoiDung
                };
                db.Set<TH_DUYETTHUCHIEN>().Add(New);
                sc += db.SaveChanges();
                if (sc > 0)
                {
                    sc += db.Database.ExecuteSqlCommand("Update TH_DUBAO set ID_LYDO_TD = 242 where A_TH_DUBAO =" + A_THDUBAO);
                    TH_DUBAO model = db.TH_DUBAO.FirstOrDefault(it => it.A_TH_DUBAO == A_THDUBAO);
                    if (sc>1&&IDDuyet==1&&model!=null)
                    {
                        TH_DUBAO Moi = new TH_DUBAO()
                        {
                            J_KEHOACH=model.J_KEHOACH,
                            J_TH_HOPDONG=model.J_TH_HOPDONG,
                            J_HOSOKHACHHANG=model.J_HOSOKHACHHANG,
                            SOBUOI=model.SOBUOI,
                            NGAYLAM=DateTime.Now,
                            NGAYGIAOHANG=model.NGAYGIAOHANG,
                            NGAYTHANHTOAN=model.NGAYTHANHTOAN,
                            ID_LYDO_HV=97,
                            ID_LYDO_TD=238,
                            SODONHANG="123",
                            ID_DTTC=FCVDataProvider.GetUserID(User.Identity.Name),
                            TK_EC=model.TK_EC,
                            TK_EC_LEADER=model.TK_EC_LEADER,
                            TK_CM=model.TK_CM,
                            TK_CM_LEADER=model.TK_CM_LEADER,
                            TK_GDTT=model.TK_GDTT,
                            TK_GDV=model.TK_GDV,
                            ID_TRUNGTAM_DI=model.ID_TRUNGTAM_DI,
                            ID_TRUNGTAM_DEN=model.ID_TRUNGTAM_DEN,
                            DULIEUNHOM=true
                        };
                        db.Set<TH_DUBAO>().Add(Moi);
                        sc += db.SaveChanges();
                        if (sc > 2)
                        {
                            int th_dubao = db.TH_DUBAO.OrderByDescending(x => x.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
                            TH_DUBAO_SANPHAM dbsp = db.TH_DUBAO_SANPHAM.FirstOrDefault(it => it.J_TH_DUBAO == A_THDUBAO);
                            TH_DUBAO_SANPHAM newdubaosp = new TH_DUBAO_SANPHAM()
                            {
                                J_TH_DUBAO=th_dubao,
                                J_SANPHAM=dbsp.J_SANPHAM,
                                SOLUONG=dbsp.SOLUONG,
                                DONGIA=dbsp.DONGIA,
                                THANHTIEN=dbsp.DONGIA
                            };
                            db.Set<TH_DUBAO_SANPHAM>().Add(newdubaosp);
                            sc += db.SaveChanges();
                        }
                    }
                }
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}