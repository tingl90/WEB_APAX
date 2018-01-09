using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DangKyLopController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        // GET: DangKyLop

        public ActionResult ChiTietDangKyLop(int? HocVien)
        {
            ViewBag.HocVien = HocVien;
            return View();
        }
        public ActionResult GetDanhSachHocSinh()
        {
            try
            {
                var model = db.APAX_THONGTIN_HS_GETDANGKYHOC(0, "", TaiKhoan).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDanhSachHocVienTheoTuKhoa(string TuKhoa)
        {
            try
            {
                var model = db.APAX_THONGTIN_HS_GETDANGKYHOC(0, TuKhoa, TaiKhoan).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDanhSachChuongTrinh()
        {
            try
            {
                var model = db.SP_APAX_GETDANHSACHCHUONGTRINH("").ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> GetThongTinGoiPhi(string TenTrungTam, string TenChuongTrinh, int? ID_GOIPHI)
        {
            try
            {
                var model = await Task.Run(() => db.APAX_DANGKYHOC_DM_GOIPHI(TenTrungTam, TenChuongTrinh).Where(t => t.ID_GOIPHI == ID_GOIPHI).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public async Task<ActionResult> LoadHocSinhInfo(int? ID)
        {
            try
            {
                var model = await Task.Run(() => db.APAX_THONGTIN_HS_GETDANGKYHOC(ID, "", TaiKhoan).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public async Task<ActionResult> LoadChuongTrinhInfo(string ID)
        {
            try
            {
                var model = await Task.Run(() => db.SP_APAX_GETDANHSACHCHUONGTRINH(ID).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult LoaiKhachHang()
        {
            var model = db.DM_LOAIHOPDONG.OrderBy(t => t.STT).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GoiPhi(string TenTrungTam, string TenChuongTrinh)
        {
            var model = db.APAX_DANGKYHOC_DM_GOIPHI(TenTrungTam, TenChuongTrinh).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadLichSuDangKy(int HocVien)
        {
            var model = db.SP_APAX_LICHSUDANGKY(HocVien).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CapNhatCM(int? A_HOSOKHACHHANG, string MADTTC)
        {
            try
            {
                var model = db.TH_HOSOKHACHHANG.FirstOrDefault(t => t.A_HOSOKHACHHANG == A_HOSOKHACHHANG);
                if (model != null)
                {
                    model.J_CSO = MADTTC;
                }
                return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult CapNhatHopDong(
            int J_HOSOKHACHHANG,
            string NGAYKYHOPDONG,
            string NGAYBATDAU,
            string TONGTIEN_HD,
            int? ID_LOAIHOPDONG,
            int J_SANPHAM,
            int SOLUONG,
            string DONGIA,
            int? CHIETKHAU,
            int? THANHTIEN,
            int? J_SALE_LEADER,
            int? J_GD_TRUNGTAM,
            int? J_GD_VUNG,
            int J_TH_BAOGIA,
            string TENGOIPHI,
            string EC,
            string EC_LEADER,
            string CM,
            string OM,
            string GDTT,
            string GDV,
            string GHICHU,
            int? ID_BOPHAN
          )
        {
            try
            {
                int sc = 0;
                var ngaybatdau = DateTime.Parse(NGAYBATDAU);
                var nguoidung = db.DM_DTTC.FirstOrDefault(t => t.TAIKHOAN == TaiKhoan).ID_DTTC;
                var modelTaiPhi = db.APAX_THONGTIN_XEPLOP(J_HOSOKHACHHANG).FirstOrDefault();
                if (ID_LOAIHOPDONG == 1044 && modelTaiPhi == null) // Không thể tái phí xuất thông báo
                {
                    return Json(-1, JsonRequestBehavior.AllowGet);
                }

                // Insert hop dong moi
                int solandataiphi = db.TH_DUBAO.Where(t => t.J_HOSOKHACHHANG == J_HOSOKHACHHANG && t.ID_LYDO_HV == 103).ToList().Count();
                db.TH_HOPDONG.Add(new TH_HOPDONG()
                {
                    J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                    J_TH_BAOGIA = J_TH_BAOGIA,
                    NGAYKYHOPDONG = DateTime.Parse(NGAYKYHOPDONG),
                    NGAYBATDAU = DateTime.Parse(NGAYBATDAU),
                    SOBUOI = SOLUONG,
                    TONGTIEN_HD = TONGTIEN_HD == "" ? 0 : decimal.Parse(TONGTIEN_HD),
                    ID_LOAIHOPDONG = ID_LOAIHOPDONG,
                    ID_DTTC = nguoidung,
                    NOIDUNGHOPDONG = TENGOIPHI,
                    TK_EC = EC,
                    TK_CM = CM,
                    TK_GDTT = GDTT,
                    TK_GDV = GDV,
                    TK_EC_LEADER = EC_LEADER,
                    TK_CM_LEADER = OM,
                    SOHOPDONG = "1111",
                    J_DTDTC = 1,
                    DULIEUNHOM = true,
                    J_SALE_LEADER = J_SALE_LEADER,
                    J_GD_TRUNGTAM = J_GD_TRUNGTAM,
                    J_GD_VUNG = J_GD_VUNG,
                    SOTANGLUYKEHD = (solandataiphi + 1).ToString()
                });
                sc += db.SaveChanges();

                int a_th_hopdong = db.TH_HOPDONG.OrderByDescending(t => t.A_TH_HOPDONG).FirstOrDefault().A_TH_HOPDONG;
                db.TH_HOPDONG_SP.Add(new TH_HOPDONG_SP()
                {
                    J_TH_HOPDONG = a_th_hopdong,
                    J_SANPHAM = J_SANPHAM,
                    SOLUONG = decimal.Parse(SOLUONG.ToString()),
                    DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA),
                    CHIETKHAU = CHIETKHAU,
                    THANHTIEN = TONGTIEN_HD == "" ? 0 : decimal.Parse(TONGTIEN_HD),
                    NOTE1 = GHICHU
                });
                sc += db.SaveChanges();

                if (ID_LOAIHOPDONG != 1044) //Học thử, Chính thức, Tái phí khác lớp
                {
                    db.Database.ExecuteSqlCommand("UPDATE TH_HOSOKHACHHANG SET SOBUOIHOC = " + SOLUONG + " WHERE A_HOSOKHACHHANG = " + J_HOSOKHACHHANG);
                    sc += db.SaveChanges();
                }

                // Cập nhật TH_DUBAO and TH_DUBAO_SANPHAM
                if (modelTaiPhi != null && ID_LOAIHOPDONG == 1044) //Tái phí cùng lớp
                {
                    var tongsobuoi = modelTaiPhi.SoBuoi + SOLUONG;
                    var modelDuBao = db.TH_DUBAO.FirstOrDefault(t => t.A_TH_DUBAO == modelTaiPhi.A_TH_DUBAO);
                    modelDuBao.NGAYLAM = DateTime.Now;
                    modelDuBao.NGAYGIAOHANG = DateTime.Parse(NGAYBATDAU);
                    //modelDuBao.ID_LYDO_HV = 97;
                    modelDuBao.NGAYTHANHTOAN = LayNgayKetThucDateTime(modelTaiPhi.NgayHocCuoi, tongsobuoi, modelTaiPhi.Buoi1, modelTaiPhi.Buoi2, modelTaiPhi.ID_KHUVUC);
                    //modelDuBao.ID_LYDO_TD = 237; 
                    modelDuBao.TK_EC = EC;
                    modelDuBao.TK_CM = CM;
                    modelDuBao.TK_GDTT = GDTT;
                    modelDuBao.TK_GDV = GDV;
                    modelDuBao.TK_EC_LEADER = EC_LEADER;
                    modelDuBao.TK_CM_LEADER = OM;
                    modelDuBao.DONGIA = DONGIA == null ? 0 : decimal.Parse(DONGIA);
                    modelDuBao.SOBUOI = tongsobuoi;
                    modelDuBao.TONGTIEN_DH = modelTaiPhi.ThanhTien + THANHTIEN;
                    modelDuBao.ID_DTTC = nguoidung;
                    sc += db.SaveChanges();

                    db.Database.ExecuteSqlCommand("Update TH_HOSOKHACHHANG set SOBUOIHOC = " + tongsobuoi + " where A_HOSOKHACHHANG = " + J_HOSOKHACHHANG);
                    sc += db.SaveChanges();

                    var ngaythanhtoan = db.TH_DUBAO.FirstOrDefault(t => t.A_TH_DUBAO == modelTaiPhi.A_TH_DUBAO).NGAYTHANHTOAN;
                    var modelDuBaoSanPham = db.TH_DUBAO_SANPHAM.FirstOrDefault(t => t.J_TH_DUBAO == modelTaiPhi.A_TH_DUBAO && t.J_SANPHAM == J_SANPHAM);
                    modelDuBaoSanPham.SOLUONG = decimal.Parse(tongsobuoi.ToString());
                    modelDuBaoSanPham.DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA);
                    modelDuBaoSanPham.CHIETKHAU = CHIETKHAU;
                    modelDuBaoSanPham.THANHTIEN = modelTaiPhi.ThanhTien + THANHTIEN;
                    modelDuBaoSanPham.DATENOTE1 = DateTime.Parse(NGAYBATDAU);
                    modelDuBaoSanPham.DATENOTE2 = ngaythanhtoan;
                    sc += db.SaveChanges();

                    //Insert đơn hàng cảnh báo tái phí
                    db.TH_DUBAO.Add(new TH_DUBAO()
                    {
                        J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                        J_TH_HOPDONG = a_th_hopdong,
                        J_KEHOACH = modelTaiPhi.A_KEHOACH,
                        SOTANGLUYKEDB = modelTaiPhi.A_TH_DUBAO.ToString(),
                        NGAYLAM = DateTime.Parse(NGAYKYHOPDONG),
                        NGAYGIAOHANG = DateTime.Parse(NGAYBATDAU),
                        NGAYTHANHTOAN = ngaythanhtoan,
                        ID_BOPHAN = ID_BOPHAN,
                        ID_LYDO_HV = 103,
                        ID_DTTC = nguoidung
                    });
                    sc += db.SaveChanges();

                }
                else  //Chính thức, tái phí khác lớp
                {
                    db.TH_DUBAO.Add(new TH_DUBAO()
                    {
                        J_HOSOKHACHHANG = J_HOSOKHACHHANG,
                        J_TH_HOPDONG = a_th_hopdong,
                        NGAYLAM = DateTime.Parse(NGAYKYHOPDONG),
                        NGAYGIAOHANG = DateTime.Parse(NGAYBATDAU),
                        ID_BOPHAN = ID_BOPHAN,
                        ID_LYDO_HV = 97,
                        ID_LYDO_TD = 237,
                        TK_EC = EC,
                        TK_CM = CM,
                        TK_GDTT = GDTT,
                        TK_GDV = GDV,
                        TK_EC_LEADER = EC_LEADER,
                        TK_CM_LEADER = OM,
                        DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA),
                        SOBUOI = SOLUONG,
                        TONGTIEN_DH = THANHTIEN,
                        ID_DTTC = nguoidung
                    });
                    sc += db.SaveChanges();

                    int j_th_dubao = db.TH_DUBAO.OrderByDescending(t => t.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
                    db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
                    {
                        J_TH_DUBAO = j_th_dubao,
                        J_SANPHAM = J_SANPHAM,
                        SOLUONG = SOLUONG,
                        DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA),
                        CHIETKHAU = CHIETKHAU,
                        THANHTIEN = THANHTIEN,
                        DATENOTE1 = DateTime.Parse(NGAYBATDAU),
                    });
                    db.Database.ExecuteSqlCommand("Update TH_HOSOKHACHHANG set SOBUOIHOC = " + SOLUONG + " where A_HOSOKHACHHANG = " + J_HOSOKHACHHANG);
                    sc += db.SaveChanges();
                }
                if (sc > 0)
                    return Json(a_th_hopdong, JsonRequestBehavior.AllowGet);
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string tb = ex.Message;
                throw;
            }
        }

        public DateTime LayNgayKetThucDateTime(DateTime? ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                string ngaybatdau = ngaybd.Value.ToString("yyyy-MM-dd");
                var model = db.APAX_HamLayNgayKetThuc(ngaybd.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2, SoBuoi, KhuVuc).ToList();
                return model[0].Date.Value;
            }
            catch
            {
                return DateTime.Now;
            }
        }

        #region Dung
        public JsonResult LoadChiTietLichSuDangKyLop(int? J_TH_HOPDONG)
        {
            var model = db.SP_APAX_CHITIETLICHSUDANGKY(J_TH_HOPDONG).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachLichSuDuyet(int? DuBao)
        {
            var model = db.APAX_DANHSACHLICHSUDUYETDANGKYLOP(DuBao).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult SaveDuyetHuyDangKy(int? A_THHOPDONG, int? IDDuyet, string NoiDung)
        {
            int sc = 0;
            try
            {
                TH_DUYETTHUCHIEN New = new TH_DUYETTHUCHIEN()
                {
                    J_TH_HOPDONG = A_THHOPDONG,
                    ID_DTTC = clsFunctions.GetUserID(),
                    NGAYDUYET = DateTime.Now,
                    ID_DUYET = IDDuyet,
                    YKIENBOSUNG = NoiDung
                };
                db.Set<TH_DUYETTHUCHIEN>().Add(New);
                sc += db.SaveChanges();
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult LoadNguoiDung(int? DuBao)
        {
            var NguoiDung = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(User.Identity.Name));
            try
            {
                var model = db.APAX_DANHSACHLICHSUDUYETDANGKYLOP(DuBao).FirstOrDefault();
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
        #endregion
    }
}