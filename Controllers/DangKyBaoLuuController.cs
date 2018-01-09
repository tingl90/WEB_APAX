using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DangKyBaoLuuController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        // GET: DangKyBaoLuu
        public ActionResult ChiTietBaoLuu()
        {
            return View();
        }
        public JsonResult GetDanhSachHocSinhDaXepLop(string HocVien)
        {
            var model = db.SP_APAX_DANHSACHDAXEPLOP(HocVien, TaiKhoan.Trim()).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult LoadDanhSachHocSinhDaXepLop(int HocVien, int LopHoc)
        {
            var model = db.SP_APAX_GETDANHSACHDAXEPLOP(HocVien, LopHoc).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChiTietLichSuBaoLuu(int? A_TH_DUBAO)
        {
            var model = db.SP_APAX_GRIDCHITIETHOCVIENBAOLUU(A_TH_DUBAO).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SoBuoiDaBaoLuu(int MaKhachHang, int MaKeHoach, int A_TH_DUBAO)
        {
            var model = db.SP_SoBuoiDaBaoLuu(MaKhachHang, MaKeHoach, A_TH_DUBAO).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDanhSachHocSinhDaBaoLuu(int HocVien, int LopHoc)
        {
            var model = db.SP_APAX_GRIDHOCVIENDABAOLUU(HocVien, LopHoc, User.Identity.Name).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CapNhatBaoLuuKetQua(
            int A_THDUBAO, 
            int J_HOSOKHACHHANG, 
            int J_KEHOACH, 
            DateTime? NGAYLAM,
            DateTime? DENNGAY,
            int SOBUOI, 
            int TONGSOBUOIDABAOLUU, 
            DateTime? NGAYHOCCUOI, 
            int ID_LYDO_NT, 
            int J_SANPHAM, 
            int SOBUOIBAOLUU, 
            string GHICHUDUBAO)
        {
            int sc = 0;
            var model2 = db.TH_DUBAO.FirstOrDefault(x => x.A_TH_DUBAO == A_THDUBAO);
            if(DateTime.Compare((DateTime)model2.NGAYTHANHTOAN,(DateTime)NGAYLAM) <= 0)
            {
                return Json("Ngày bảo lưu không được phép sau ngày học cuối", JsonRequestBehavior.AllowGet);
            }
            if (DateTime.Compare((DateTime)NGAYLAM, (DateTime)model2.NGAYGIAOHANG) <= 0)
            {
                return Json("Ngày bảo lưu không được phép trước ngày học chính thức và ngày dự kiến học", JsonRequestBehavior.AllowGet);
            }
            if (model2.NGAY1!=null&& (DateTime.Compare((DateTime)NGAYLAM, (DateTime)model2.NGAY1) <= 0))
            {
                return Json("Ngày bảo lưu không được phép trong khoản thời gian bảo lưu trước đó", JsonRequestBehavior.AllowGet);
            }
            var thdubao = new TH_DUBAO();
            thdubao.GHICHU = GHICHUDUBAO;
            thdubao.J_HOSOKHACHHANG = J_HOSOKHACHHANG;
            thdubao.J_KEHOACH = J_KEHOACH;
            thdubao.NGAYLAM = DateTime.Now;
            thdubao.SOBUOI = SOBUOI;
            thdubao.TONGSOBUOIBAOLUU = SOBUOIBAOLUU;
            thdubao.ID_DTTC = FCVDataProvider.GetUserID(User.Identity.Name);
            thdubao.NGAYGIAOHANG = NGAYLAM;
            thdubao.NGAYTHANHTOAN =  DENNGAY;
            thdubao.ID_LYDO_HV = 98;
            thdubao.ID_LYDO_NT = ID_LYDO_NT;
            db.TH_DUBAO.Add(thdubao);
            db.SaveChanges();

            var a_th_dubao = db.TH_DUBAO.OrderByDescending(x => x.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
            var thdubaosanpham = new TH_DUBAO_SANPHAM();
            thdubaosanpham.J_TH_DUBAO = a_th_dubao;
            thdubaosanpham.J_SANPHAM = J_SANPHAM == null ? 0 : J_SANPHAM;
            thdubaosanpham.SOLUONG = SOBUOIBAOLUU;
            thdubaosanpham.DATENOTE = model2.NGAYTHANHTOAN;
            db.TH_DUBAO_SANPHAM.Add(thdubaosanpham);
            db.SaveChanges();
            model2.TONGSOBUOIBAOLUU = (model2.TONGSOBUOIBAOLUU>0? model2.TONGSOBUOIBAOLUU:0)+ SOBUOIBAOLUU;
            model2.NGAYTHANHTOAN = NGAYHOCCUOI;
            model2.NGAY1 = DENNGAY;
            sc += db.SaveChanges();
            if (sc > 0)
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        HttpPostedFileBase avtFileInbox = Request.Files[0];
                        if (avtFileInbox != null && avtFileInbox.ContentLength > 0)
                        {
                            try
                            {
                                var fileName = Path.GetFileName(avtFileInbox.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content/upload"), fileName);
                                TH_DUBAO_FILE DuLieuFile = new TH_DUBAO_FILE()
                                {
                                    FILENAME = fileName,
                                    J_TH_DUBAO = a_th_dubao,
                                    PATHFILE = path,
                                };
                                db.Set<TH_DUBAO_FILE>().Add(DuLieuFile);
                                int kc = db.SaveChanges();
                                if (kc > 0)
                                {
                                    avtFileInbox.SaveAs(path);
                                }
                            }
                            catch { }
                        }
                    }
                    catch
                    {

                    }
                }
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }
        public ActionResult XemFile(int? tenfile)
        {
            var model = db.TH_DUBAO_FILE.FirstOrDefault(it => it.J_TH_DUBAO == tenfile);
            if(model!=null && model.FILENAME!=null)
                ViewBag.TenHinh = model.FILENAME;
            else
                ViewBag.TenHinh = "";
            return View();
        }

        #region Tính ngày kết thúc

        public ActionResult LayNgayKetThuc2(string ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2,int? KhuVuc)
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
                var modelNgayLe = db.HT_CONFIG_HOLIDAY.Where(x=>x.ID_KHUVUC==KhuVuc).ToList();
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
                return Json(NgayKetThuc.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(7, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LayNgayKetThuc3(DateTime? ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                DateTime? NgayBatDau = ngaybd;
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
                return Json(NgayKetThuc.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(7, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LayNgayKetThuc4(DateTime? ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                DateTime NgayKetThuc = ngaybd.Value;
                //if (SoBuoi == 1)
                //{
                //    return Json(NgayKetThuc, JsonRequestBehavior.AllowGet);
                //}
                var modelNgayLe = db.HT_CONFIG_HOLIDAY.Where(x => x.ID_KHUVUC == KhuVuc).ToList();
                if (Buoi2 == null || Buoi1 == Buoi2)
                {
                    for (int j = 2; j <= SoBuoi; j++)
                    {
                        if (modelNgayLe.Where(t => t.DAY.ToString("dd/MM/yyyy") == NgayKetThuc.ToString("dd/MM/yyyy")).Count() > 0) j--; // Ngày kết thúc thuộc ngày lễ thì trừ bớt biến i trong số buổi
                        NgayKetThuc = NgayKetThuc.AddDays(7);
                    }
                }
                int thuNho = 0;
                int thuLon = 0;
                if (Buoi1 < Buoi2)
                {
                    thuNho = (int)Buoi1;
                    thuLon = (int)Buoi2;
                }
                else if (Buoi1 > Buoi2)
                {
                    thuNho = (int)Buoi2;
                    thuLon = (int)Buoi1;
                }
                int soNgayTrongTuan = thuLon - thuNho;
                int soNgayNgoaiTuan = 7 - thuLon + thuNho;
                for (int i = 1; i <= SoBuoi; i++)
                {
                    if (LayThu(NgayKetThuc.DayOfWeek.ToString()) == thuNho) //Ngày kết thúc rơi vào thứ nhỏ
                    {
                        NgayKetThuc = NgayKetThuc.AddDays(soNgayTrongTuan);
                    }
                    else if (LayThu(NgayKetThuc.DayOfWeek.ToString()) == thuLon) //Ngày kết thúc rơi vào thứ lơn
                    {
                        NgayKetThuc = NgayKetThuc.AddDays(soNgayNgoaiTuan);
                    }
                    if (modelNgayLe.Where(t => t.DAY.ToString("dd/MM/yyyy") == NgayKetThuc.ToString("dd/MM/yyyy")).Count() > 0) i--; // Ngày kết thúc thuộc ngày lễ thì trừ bớt biến i trong số buổi
                }
                return Json(NgayKetThuc.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(7, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LayNGayKetThucHoanChinh(DateTime? ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                var model = db.APAX_HamLayNgayKetThuc(ngaybd.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2,SoBuoi, KhuVuc).FirstOrDefault();
                return Json(model.Date.Value.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LayNGayKetThucHoanChinhDaTinhNgay(DateTime? ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
        {
            try
            {
                var model = db.APAX_HamLayNgayKetThucBaoLuu(ngaybd.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2, SoBuoi, KhuVuc).FirstOrDefault();
                return Json(model.Date.Value.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
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

        #endregion

    }
}