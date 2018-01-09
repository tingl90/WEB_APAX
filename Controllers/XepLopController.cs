using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class XepLopController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        // GET: XepLop
        public ActionResult ChiTietXepLop()
        {
            ViewBag.TrungTam = TaiKhoan.Equals("Administrator") ? 0 : db.Database.SqlQuery<int>(@"SELECT ID_TRUNGTAM FROM dbo.DM_TRUNGTAM WHERE MA_DVCS = (SELECT MATRUNGTAM FROM dbo.DM_DTTC WHERE TAIKHOAN = '" + TaiKhoan + "')").FirstOrDefault();
            return View();
        }
        public ActionResult GetDanhSachCho(int? TrungTam, int? ChuongTrinh)
        {
            try
            {
                var model = db.SP_APAX_GETDANHSACHXEPLOP(TrungTam, ChuongTrinh, TaiKhoan).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDanhSachDangHoc(int? LopHoc)
        {
            try
            {
                if (LopHoc == null) LopHoc = 0;
                var model = db.SP_APAX_GETDANHSACHDACTIVE(238, LopHoc, TaiKhoan).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetDanhSachLopHoc(int? TrungTam, int? ChuongTrinh)
        {
            try
            {
                var model = db.SP_APAX_DANHSACHLOP_XEPLOP(TrungTam, ChuongTrinh, TaiKhoan, 0).ToList();
                var json = Json(model, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = int.MaxValue;
                return json;
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> LoadLopHocInfo(int? ID)
        {
            try
            {
                var model = await Task.Run(() => db.SP_APAX_DANHSACHLOP_XEPLOP(0, 0, TaiKhoan, ID).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult XepLopChoHocVien(
            string HocVien,
            int? LopHoc,
            string HopDong,
            string CM_PHUTRACH,
            string CM_LEADER,
            string GD_TRUNGTAM,
            string GD_VUNG,
            string SoBuoi,
            string DONGIA,
            string CHIETKHAU,
            string THANHTIEN,
            int Buoi1,
            int Buoi2,
            int KhuVuc,
            string NgayXepLop,
            int ChuongTrinh,
            string EC,
            string EC_LEADER,
            string CM,
            string OM,
            int? A_TH_DUBAO
            )
        {
            try
            {
                int sc = 0;
                int khachhang = int.Parse(HocVien);
                var ngaythanhtoan = LayNGayKetThucHoanChinhDateTime(NgayXepLop, int.Parse(SoBuoi), Buoi1, Buoi2, KhuVuc);

                var modelDuBao = db.TH_DUBAO.FirstOrDefault(t => t.A_TH_DUBAO == A_TH_DUBAO);
                modelDuBao.J_HOSOKHACHHANG = khachhang;
                modelDuBao.J_KEHOACH = LopHoc;
                modelDuBao.J_TH_HOPDONG = int.Parse(HopDong);
                modelDuBao.NGAYLAM = DateTime.Now;
                modelDuBao.NGAYGIAOHANG = DateTime.Parse(NgayXepLop);
                modelDuBao.NGAYTHANHTOAN = ngaythanhtoan;
                modelDuBao.ID_LYDO_HV = 97;
                modelDuBao.ID_LYDO_TD = 238;
                modelDuBao.ID_DTTC = db.DM_DTTC.FirstOrDefault(t => t.TAIKHOAN == TaiKhoan).ID_DTTC;
                modelDuBao.CM = CM_PHUTRACH;
                modelDuBao.CM_LEADER = CM_LEADER;
                modelDuBao.GD_TRUNGTAM_CM = GD_TRUNGTAM;
                modelDuBao.GD_VUNG_CM = GD_VUNG;
                modelDuBao.TK_EC = EC;
                modelDuBao.TK_EC_LEADER = EC_LEADER;
                modelDuBao.TK_CM = CM;
                modelDuBao.TK_CM_LEADER = OM;
                modelDuBao.TK_GDTT = GD_TRUNGTAM;
                modelDuBao.TK_GDV = GD_VUNG;
                sc += db.SaveChanges();

                var modelDuBaoSanPham = db.TH_DUBAO_SANPHAM.FirstOrDefault(t => t.J_TH_DUBAO == A_TH_DUBAO);

                modelDuBaoSanPham.J_SANPHAM = ChuongTrinh;
                modelDuBaoSanPham.SOLUONG = decimal.Parse(SoBuoi);
                modelDuBaoSanPham.DONGIA = DONGIA == "" ? 0 : decimal.Parse(DONGIA);
                modelDuBaoSanPham.CHIETKHAU = CHIETKHAU == "" ? 0 : int.Parse(CHIETKHAU);
                modelDuBaoSanPham.THANHTIEN = THANHTIEN == "" ? 0 : decimal.Parse(THANHTIEN);
                modelDuBaoSanPham.DATENOTE = DateTime.Parse(NgayXepLop);
                modelDuBaoSanPham.DATENOTE2 = ngaythanhtoan;

                db.Database.ExecuteSqlCommand("Update TH_HOSOKHACHHANG set SOBUOIHOC = " + SoBuoi + " where A_HOSOKHACHHANG = " + HocVien);
                sc += db.SaveChanges();
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string LayNgayKetThuc2(string ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                DateTime? NgayBatDau = DateTime.Parse(ngaybd);
                string BUOI = Buoi1 + "," + Buoi2;
                //SoBuoi = SoBuoi + 1;
                string[] Thu = BUOI.Split(',');
                DateTime NgayKetThuc = NgayBatDau.Value;
                int NgayHienTai = NgayBatDau.Value.Day, ThangHienTai = NgayBatDau.Value.Month, NamHienTai = NgayBatDau.Value.Year;
                int SoNgayTrongThang = TinhSoNgayCuaThang(NgayBatDau);
                DateTime NgayLuu = DateTime.Parse(NgayBatDau.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai));
                var modelNgayLe = db.HT_CONFIG_HOLIDAY.Where(x => x.ID_KHUVUC == KhuVuc).ToList();
                while (SoBuoi > 0)
                {
                    foreach (string tt in Thu)
                    {
                        string TenThuTiengAnh = ThuTiengAnh(int.Parse(tt));
                        if (TenThuTiengAnh.Equals(DateTime.Parse(NgayBatDau.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai)).DayOfWeek.ToString()))
                        {
                            NgayKetThuc = DateTime.Parse(NgayBatDau.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai));
                            var TonTaiNgayLe = modelNgayLe.Where(t => t.DAY.ToString("dd/MM/yyyy").Contains(NgayKetThuc.ToString("dd/MM/yyyy"))).FirstOrDefault();
                            if (TonTaiNgayLe == null)
                            {
                                SoBuoi--;
                            }
                        }
                    }
                    NgayHienTai++;
                    if (NgayHienTai > SoNgayTrongThang)
                    {
                        NgayHienTai = 1;
                        ThangHienTai++;
                    }
                    if (ThangHienTai > 12)
                    {
                        ThangHienTai = 1;
                        NamHienTai++;
                    }
                    NgayLuu = DateTime.Parse(NgayBatDau.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai));
                    SoNgayTrongThang = TinhSoNgayCuaThang(DateTime.Parse(NgayBatDau.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai)));
                }
                return NgayKetThuc.ToString("yyyy-MM-dd");
            }
            catch
            {
                return "1";
            }
        }

        public static string ThuTiengAnh(int? Thu)
        {
            string TenThu = null;
            switch (Thu)
            {
                case 1:
                    TenThu = "Sunday";
                    break;
                case 2:
                    TenThu = "Monday";
                    break;
                case 3:
                    TenThu = "Tuesday";
                    break;
                case 4:
                    TenThu = "Wednesday";
                    break;
                case 5:
                    TenThu = "Thursday";
                    break;
                case 6:
                    TenThu = "Friday";
                    break;
                case 7:
                    TenThu = "Saturday";
                    break;
                default:
                    break;
            }
            return TenThu;
        }
        public static int TinhSoNgayCuaThang(DateTime? NgayThang)
        {
            int SoNgay = 0;
            switch (NgayThang.Value.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    SoNgay = 31;
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    SoNgay = 30;
                    break;
                case 2:
                    if (NgayThang.Value.Year % 100 != 0 && NgayThang.Value.Year / 4 == 0)
                        SoNgay = 29;
                    else
                        SoNgay = 28;
                    break;
                default:
                    SoNgay = 0;
                    break;
            }
            return SoNgay;
        }
        public static int LayThu(string Thu)
        {
            int TenThu = 0;
            switch (Thu)
            {
                case "Sunday":
                    TenThu = 1;
                    break;
                case "Monday":
                    TenThu = 2;
                    break;
                case "Tuesday":
                    TenThu = 3;
                    break;
                case "Wednesday":
                    TenThu = 4;
                    break;
                case "Thursday":
                    TenThu = 5;
                    break;
                case "Friday":
                    TenThu = 6;
                    break;
                case "Saturday":
                    TenThu = 7;
                    break;
                default:
                    break;
            }
            return TenThu;

        }
        private string ChuyenDoiNgay(string ngayChon)
        {
            if (ngayChon == "")
                return string.Empty;
            string[] list = ngayChon.ToString().Substring(0, 10).Split('/');
            return list[2] + '-' + list[0] + '-' + list[1];
        }
        public ActionResult LayNGayKetThucHoanChinh(DateTime? ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
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
        public DateTime LayNGayKetThucHoanChinhDateTime(string ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                var model = db.APAX_HamLayNgayKetThuc(ngaybd, Buoi1, Buoi2, SoBuoi, KhuVuc).FirstOrDefault();
                return model.Date.Value;
            }
            catch
            {
                return DateTime.Now;
            }
        }
    }
}