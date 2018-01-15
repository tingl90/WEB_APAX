using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DangKyChuyenPhiController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        // GET: DangKyChuyenPhi
        public ActionResult ChiTietChuyenPhi()
        {
            return View();
        }
        public JsonResult GetDanhSachHocSinh(int? Loai,int? ID_HocSinhCho,int? ID_HocSinhNhan)
        {
            try
            {
                if (Loai==1)
                {
                    var model = db.SP_APAX_TIMKIEM_DANHSACH_DAXEPLOP(0, TaiKhoan, ID_HocSinhNhan).ToList();
                    var json = Json(model, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                }
                else
                {
                    var model = db.SP_APAX_TIMKIEM_DANHSACH_NHANPHI(TaiKhoan, ID_HocSinhCho).ToList();
                    var json = Json(model, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                }
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
                var model = await Task.Run(() => db.SP_APAX_TIMKIEM_DANHSACH_DAXEPLOP(ID,TaiKhoan,0).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public async Task<ActionResult> LoadHocSinhNhanInfo(int? ID)
        {
            try
            {
                var model = await Task.Run(() => db.SP_APAX_CHITIETHOCSINHNHANHOCPHI(ID, TaiKhoan).FirstOrDefault());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public JsonResult LoadLichSuChuyenPhi(int HocVien)
        {
            var model = db.SP_APAX_GRIDLICHSUCHUYENPHI1(HocVien, User.Identity.Name).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChiTietLichSuChuyenPhi(int? A_TH_DUBAO)
        {
            var model = db.SP_APAX_GETCHITIETHOCSINHCHUYENNHANPHI("A",A_TH_DUBAO).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        private string ChuyenDoiNgay(string ngayChon)
        {
            if (ngayChon == "")
                return string.Empty;
            string[] list = ngayChon.ToString().Substring(0, 10).Split('/');
            return list[2] + '-' + list[1] + '-' + list[0] + " 00:00:00";
        }
        public int UpdateChuyenPhi(
            int A_HOSOKHACHHANG_CHUYEN,
            int? A_KEHOACH_CHUYEN,
            int A_HOSOKHACHHANG_NHAN,
            int? A_KEHOACH_NHAN,
            decimal? SOTIEN,
            int? SOBUOICON,
            decimal DONGIABUOI,
            int? A_SANPHAM_NHAN,
            int A_TH_HOPDONG_NHAN,
            int? A_SANPHAM_CHUYEN,
            int A_TH_HOPDONG_CHUYEN,
            DateTime? NGAYBATDAU_CHUYEN,
            DateTime? NGAYCHUYEN,
            DateTime? NGAYBATDAU_NHAN,
            DateTime? NHAYKETTHUC_NHAN,
            int? SOBUOINHAN,
            decimal? DONGIACHUYEN,
            decimal? DONGIANHAN,
            decimal? ThanhTien,
            int? HinhThucChuyenPhi,
            int? A_DUBAO_XEPLOP)
        {
            try
            {
                int sc = 0,ec=0;
                APAXEntities3 db = new APAXEntities3();
                db.TH_DUBAO.Add(new TH_DUBAO()
                {
                    J_HOSOKHACHHANG = A_HOSOKHACHHANG_CHUYEN,
                    J_TH_HOPDONG=A_TH_HOPDONG_CHUYEN,
                    ID_DTTC = FCVDataProvider.GetUserID(User.Identity.Name),
                    DULIEUNHOM = true,
                    J_KEHOACH = A_KEHOACH_CHUYEN,
                    NGAYLAM = DateTime.Now,
                    NGAYGIAOHANG= NGAYBATDAU_CHUYEN,
                    NGAYTHANHTOAN=NGAYCHUYEN,
                    SOBUOI = SOBUOICON,
                    TONGTIEN_DH = SOTIEN,
                    ID_LYDO_HV = 99,
                    ID_THAIDO=HinhThucChuyenPhi,
                    ID_LYDO_TD=241
                });
                sc+=db.SaveChanges();
                if(sc>0)
                {
                    int a_th_dubao = db.TH_DUBAO.OrderByDescending(t => t.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
                    db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
                    {
                        J_TH_DUBAO = a_th_dubao,
                        J_SANPHAM = A_SANPHAM_CHUYEN>0?(int)A_SANPHAM_CHUYEN:0,
                        SOLUONG = SOBUOICON == null ? 0 : decimal.Parse(SOBUOICON.ToString()),
                        DONGIA = SOTIEN,
                        THANHTIEN = SOTIEN*(SOBUOICON == null ? 0 : decimal.Parse(SOBUOICON.ToString())),
                    });
                    sc+= db.SaveChanges();
                    if (sc>=2)
                    {
                        db.TH_DUBAO.Add(new TH_DUBAO()
                        {
                            J_HOSOKHACHHANG = A_HOSOKHACHHANG_NHAN,
                            J_TH_HOPDONG = A_TH_HOPDONG_NHAN,
                            ID_DTTC = FCVDataProvider.GetUserID(User.Identity.Name),
                            DULIEUNHOM = true,
                            J_KEHOACH = A_KEHOACH_NHAN,
                            NGAYLAM = DateTime.Now,
                            NGAYGIAOHANG = NGAYBATDAU_NHAN,
                            NGAYTHANHTOAN = NHAYKETTHUC_NHAN,
                            SOBUOI = SOBUOICON,
                            TONGTIEN_DH = SOTIEN,
                            ID_THAIDO=HinhThucChuyenPhi,
                            ID_LYDO_HV = 1103,
                            ID_LYDO_TD = 241,
                            A_THUCHIEN=a_th_dubao
                        });
                        ec += db.SaveChanges();
                        if (ec > 0)
                        {
                            int a_th_dubaonhan = db.TH_DUBAO.OrderByDescending(t => t.A_TH_DUBAO).FirstOrDefault().A_TH_DUBAO;
                            db.TH_DUBAO_SANPHAM.Add(new TH_DUBAO_SANPHAM()
                            {
                                J_TH_DUBAO = a_th_dubaonhan,
                                J_SANPHAM = A_SANPHAM_NHAN>0?(int)A_SANPHAM_NHAN:0,
                                SOLUONG = SOBUOINHAN == null ? 0 : decimal.Parse(SOBUOINHAN.ToString()),
                                DONGIA = DONGIANHAN,
                                THANHTIEN = DONGIANHAN*(SOBUOINHAN == null ? 0 : decimal.Parse(SOBUOINHAN.ToString())),
                            });
                            ec += db.SaveChanges();
                            TH_DUBAO DUBAO = db.TH_DUBAO.FirstOrDefault(st=>st.A_TH_DUBAO==A_DUBAO_XEPLOP);
                            DUBAO.ID_TRANGTHAI = 1;
                            db.Entry(DUBAO).State = EntityState.Modified;
                            sc+=db.SaveChanges();
                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                //db.TH_THUTIEN.Add(new TH_THUTIEN()
                //{
                //    J_HOSOKHACHHANG = A_HOSOKHACHHANG_NHAN,
                //    J_TH_HOPDONG = A_TH_HOPDONG_NHAN,
                //    NGAYTHUTIEN = DateTime.Now,
                //    SOTIENTHU = SOTIEN,
                //    ID_LOAIPHIEUTHU = 3,
                //    ID_DTTC = FCVDataProvider.GetUserID(User.Identity.Name),
                //    MATHUTIEN = "1111",
                //    DULIEUNHOM = true
                //});
                //sc += db.Database.ExecuteSqlCommand("UPDATE dbo.TH_DUBAO SET NGAYTHANHTOAN = GETDATE(), ID_LYDO_TD = 240 WHERE J_HOSOKHACHHANG= '" + A_HOSOKHACHHANG_CHUYEN + "' AND J_KEHOACH = '" + A_KEHOACH_CHUYEN + "'");
                //db.SaveChanges();
                return sc;
            }
            catch
            {
                return 0;
            }
        }
        public ActionResult GetDanhSachDuyetChuyenPhi()
        {
            var model = db.SP_APAX_GETDANHSACHCHUYENPHIDUYET(TaiKhoan,0).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult LoadThongTinhDuyetChuyenPhi(int? DuBao)
        {
            var NguoiDung = db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(User.Identity.Name));
            try
            {
                var model = db.SP_APAX_GETDANHSACHCHUYENPHIDUYET(User.Identity.Name, DuBao).FirstOrDefault();
                if (model != null)
                {
                    if(model.TENDTTC==null)
                    {
                        model.TENDTTC = NguoiDung.TENDTTC;
                    }
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
        public JsonResult GetDanhSachLichSuDuyet(int? DuBao)
        {
            var model = db.APAX_DANHSACHLICHSUDUYET(DuBao).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
        public JsonResult SaveDuyetChuyenPhi(int? A_THDUBAO, int? IDDuyet, string NoiDung,int? Buoi1,int? Buoi2,int? KhuVuc,int? A_DuBao_XepLopChuyen, int? A_DuBao_XepLopNhan)
        {
            int sc = 0;
            try
            {
                TH_DUBAO model = db.TH_DUBAO.FirstOrDefault(it => it.A_THUCHIEN == A_THDUBAO);//hoc sinh nhan phi
                TH_DUBAO model1 = db.TH_DUBAO.FirstOrDefault(it => it.A_TH_DUBAO == A_THDUBAO);//hoc sinh chuyen phi
                if (model1 != null)
                {
                    //them duyet vao hoc sinh chuyen
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
                        sc += db.Database.ExecuteSqlCommand("Update TH_DUBAO set ID_LYDO_TD = 242 where A_TH_DUBAO ="+A_THDUBAO);
                    }
                    //them duyet vao hoc sinh nhan
                    TH_DUYETTHUCHIEN New1 = new TH_DUYETTHUCHIEN()
                    {
                        J_TH_DUBAO = model.A_TH_DUBAO,
                        ID_DTTC = clsFunctions.GetUserID(),
                        NGAYDUYET = DateTime.Now,
                        ID_DUYET = IDDuyet,
                        YKIENBOSUNG = NoiDung
                    };
                    db.Set<TH_DUYETTHUCHIEN>().Add(New1);
                    sc += db.SaveChanges();
                    if(sc>1)
                    {
                        sc += db.Database.ExecuteSqlCommand("Update TH_DUBAO set ID_LYDO_TD = 242 where A_TH_DUBAO =" + model.A_TH_DUBAO);
                    }
                    if (IDDuyet == 1)
                    {
                        //update vao du bao xep lop hoc sinh chuyen
                        TH_DUBAO HSChuyen = db.TH_DUBAO.FirstOrDefault(it => it.A_TH_DUBAO == A_DuBao_XepLopChuyen);
                        HSChuyen.NGAYTHANHTOAN = model1.NGAYTHANHTOAN;
                        HSChuyen.SOBUOI = HSChuyen.SOBUOI-model1.SOBUOI;
                        HSChuyen.TONGTIEN_DH = HSChuyen.TONGTIEN_DH-model1.TONGTIEN_DH;
                        HSChuyen.ID_TRANGTHAI = 1;
                        db.Entry(HSChuyen).State = EntityState.Modified;
                        sc += db.SaveChanges();
                        //update dubao_sanphamn cho don hang chuyen phi
                        TH_DUBAO_SANPHAM THDBSPChuyen = db.TH_DUBAO_SANPHAM.FirstOrDefault(k => k.J_TH_DUBAO == A_DuBao_XepLopChuyen);
                        THDBSPChuyen.SOLUONG = THDBSPChuyen.SOLUONG-(decimal)model1.SOBUOI;
                        THDBSPChuyen.THANHTIEN = THDBSPChuyen.THANHTIEN-model1.TONGTIEN_DH;
                        db.Entry(THDBSPChuyen).State = EntityState.Modified;
                        sc += db.SaveChanges();
                        //update vao du bao xep lop cua hoc sinh nhan
                        TH_DUBAO HSNhan = db.TH_DUBAO.FirstOrDefault(it => it.A_TH_DUBAO == A_DuBao_XepLopNhan);
                        HSNhan.NGAYTHANHTOAN = HSNhan.J_KEHOACH != null?model1.NGAYTHANHTOAN:null;
                        HSNhan.SOBUOI = HSNhan.SOBUOI+model.SOBUOI;
                        HSNhan.TONGTIEN_DH = HSNhan.TONGTIEN_DH+model1.TONGTIEN_DH;
                        db.Entry(HSNhan).State = EntityState.Modified;
                        sc += db.SaveChanges();
                        TH_DUBAO_SANPHAM THDBSPNhan = db.TH_DUBAO_SANPHAM.FirstOrDefault(k => k.J_TH_DUBAO == A_DuBao_XepLopNhan);
                        THDBSPNhan.SOLUONG = THDBSPNhan.SOLUONG+(decimal)model.SOBUOI;
                        THDBSPNhan.THANHTIEN = THDBSPNhan.THANHTIEN+model1.TONGTIEN_DH;
                        db.Entry(THDBSPNhan).State = EntityState.Modified;
                        sc += db.SaveChanges();
                    }
                }
                return Json(sc, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public int TinhBuoiConLai(DateTime? NgayBatDau, DateTime? NgayChuyen, DateTime? NgayKetThuc, int? KhuVuc, int? HocVien, int? LopHoc, int? Buoi1, int? Buoi2, int? A_DuBao)
        {
            try
            {
                var SoBuoiBaoLuu = db.TH_DUBAO.FirstOrDefault(k => k.A_TH_DUBAO == A_DuBao);
                var model = db.APAX_DemNgayConLaiChuyenPhi(NgayChuyen.Value.ToString("yyyy-MM-dd"), NgayKetThuc.Value.ToString("yyyy-MM-dd"), Buoi1, Buoi2,KhuVuc).ToList();
                if (model.Count > 0)
                    return model.Count-((SoBuoiBaoLuu!=null&&SoBuoiBaoLuu.TONGSOBUOIBAOLUU>0)?(int)SoBuoiBaoLuu.TONGSOBUOIBAOLUU:0);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }
        public ActionResult LayNgayKetThuc2(string ngaybd, int? SoBuoi, int? Buoi1, int? Buoi2, int? KhuVuc)
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
                return Json(NgayKetThuc.ToString("dd/MM/yyyy"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(7, JsonRequestBehavior.AllowGet);
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