using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class TaiPhiController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        public ActionResult ChiTietTaiPhi()
        {
            return View();
        }

        public JsonResult GetDanhSachHocSinhDaXepLop(string HocVien)
        {
            var model = db.SP_APAX_DANHSACHDAXEPLOP(HocVien,TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult LoadDanhSachHocSinhDaXepLop(int HocVien, int LopHoc)
        {
            var model = db.SP_APAX_GETDANHSACHDAXEPLOPCANCHUYEN(HocVien, LopHoc).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveThuTaiPhi(int A_THDUBAO,int J_HOSOKHACHHANG, int J_TH_HOPDONG, string NgayThuTien, int? NguoiThuTien,
            int SoBuoi,decimal DonGia,string ThanhTien,string NgayKetThuc,int A_THDUBAO_SP,int? hinhthucthu)
        {
            var th_thuhien = new TH_THUTIEN();
            th_thuhien.J_HOSOKHACHHANG = J_HOSOKHACHHANG;
            th_thuhien.J_TH_HOPDONG = J_TH_HOPDONG;
            th_thuhien.NGAYTHUTIEN = DateTime.Parse(NgayThuTien);
            th_thuhien.SOTIENTHU = decimal.Parse(ThanhTien.Replace(",", ""));
            th_thuhien.LOAI = 2;
            th_thuhien.ID_LOAIPHIEUTHU = hinhthucthu;
            th_thuhien.ID_DTTC = NguoiThuTien != null ? (int)NguoiThuTien : 0;
            db.TH_THUTIEN.Add(th_thuhien);
            //var model = db.TH_DUBAO.FirstOrDefault(x => x.A_TH_DUBAO == A_THDUBAO);
            //if (model != null)
            //{
            //    model.NGAYTHANHTOAN = DateTime.ParseExact(NgayKetThuc, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //}
            var model2 = db.TH_HOPDONG_SP.FirstOrDefault(x => x.J_TH_HOPDONG == J_TH_HOPDONG);
            if (model2 != null)
            {
                model2.SOLUONG = SoBuoi;
            }
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }
        public int TinhBuoiConLai(DateTime? NgayBatDau, DateTime? NgayChuyen, DateTime? NgayKetThuc, int? KhuVuc, int? HocVien, int? LopHoc, int? Buoi1, int? Buoi2)
        {
            try
            {
                string BUOI = Buoi1 + "," + Buoi2;
                int SoBuoi = 0;
                int SoBuoiLe = 0;
                string[] Thu = BUOI.Split(',');
                int NgayHienTai = NgayChuyen.Value.Day, ThangHienTai = NgayChuyen.Value.Month, NamHienTai = NgayChuyen.Value.Year;
                int NgayLeHienTai = NgayBatDau.Value.Day, ThangLeHienTai = NgayBatDau.Value.Month, NamLeHienTai = NgayBatDau.Value.Year;
                int SoNgayTrongThang = TinhSoNgayCuaThang(NgayChuyen);
                int SoNgayTrongThangLe = TinhSoNgayCuaThang(NgayBatDau);
                DateTime NgayLuu = DateTime.Parse(NgayChuyen.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai));
                DateTime NgayTinhLe = DateTime.Parse(NgayBatDau.Value.ToString(NamLeHienTai + "-" + ThangLeHienTai + "-" + NgayLeHienTai));
                var modelNgayLe = db.HT_CONFIG_HOLIDAY.Where(it => it.ID_KHUVUC == KhuVuc).ToList();
                while (DateTime.Compare((DateTime)NgayTinhLe, (DateTime)NgayChuyen) <= 0)
                {
                    foreach (string tt in Thu)
                    {
                        var TonTaiNgayLe = modelNgayLe.Where(t => t.DAY.ToString("dd/MM/yyyy").Equals(NgayTinhLe.ToString("dd/MM/yyyy"))).FirstOrDefault();
                        if (TonTaiNgayLe != null)
                        {
                            SoBuoiLe++;
                        }
                    }
                    NgayLeHienTai++;
                    if (NgayLeHienTai > SoNgayTrongThangLe)
                    {
                        NgayLeHienTai = 1;
                        ThangLeHienTai++;
                    }
                    if (ThangLeHienTai > 12)
                    {
                        ThangLeHienTai = 1;
                        NamLeHienTai++;
                    }
                    NgayTinhLe = DateTime.Parse(NgayBatDau.Value.ToString(NamLeHienTai + "-" + ThangLeHienTai + "-" + NgayLeHienTai));
                    SoNgayTrongThangLe = TinhSoNgayCuaThang(DateTime.Parse(NgayChuyen.Value.ToString(NamLeHienTai + "-" + ThangLeHienTai + "-" + NgayLeHienTai)));
                }
                while (DateTime.Compare(NgayLuu, (DateTime)NgayKetThuc) <= 0)
                {
                    foreach (string tt in Thu)
                    {
                        string TenThuTiengAnh = ThuTiengAnh(int.Parse(tt));
                        if (TenThuTiengAnh.Equals(DateTime.Parse(NgayLuu.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai)).DayOfWeek.ToString()))
                        {
                            SoBuoi++;
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
                    NgayLuu = DateTime.Parse(NgayChuyen.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai));
                    SoNgayTrongThang = TinhSoNgayCuaThang(DateTime.Parse(NgayChuyen.Value.ToString(NamHienTai + "-" + ThangHienTai + "-" + NgayHienTai)));
                }
                var SoBuoiBaoLuu = db.TH_DUBAO.FirstOrDefault(it => it.J_HOSOKHACHHANG == HocVien && it.ID_LYDO_HV == 98 && it.J_KEHOACH == LopHoc && it.ID_LYDO_NT == 84);
                int sb = SoBuoiBaoLuu != null && SoBuoiBaoLuu.TONGSOBUOIBAOLUU > 0 ? (int)SoBuoiBaoLuu.TONGSOBUOIBAOLUU : 0;
                return SoBuoi + SoBuoiLe + sb;
            }
            catch
            {
                return 0;
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
    }
}