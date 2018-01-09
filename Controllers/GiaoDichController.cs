using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class GiaoDichController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        public ActionResult QuanLyGiaoDich()
        {            
            ViewBag.NguoiGiaoDich = User.Identity.Name;
            return View();
        }

        public JsonResult GetDanhSachHocSinh(string HocVien)
        {
            var model = db.SP_APAX_TIMDANHSACHHOCSINH(HocVien,TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public JsonResult LoadDanhSachHocSinh(int HocVien)
        {
            var model = db.SP_APAX_GETDANHSACHHOCSINH(HocVien).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadHocSinhGiaoDich(int HocVien)
        {
            var model = db.SP_APAX_DANHSACHHOCSINHGIAODICH(HocVien).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ChiTietGiaoDich(int HocVien,int GiaoDich)
        {
            var model = db.SP_APAX_DANHSACHHOCSINHGIAODICH(HocVien).Where(it=>it.A_TH_GIAODICH==GiaoDich).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveGiaoDich(int J_HOSOKHACHHANG, string NGAYGIAODICH,int ID_TINHTRANGGD,string NOIDUNG_GIAODICH, string CHOTCACDIEMTHONGNHAT)
        {
            
            var giaodich = new TH_GIAODICH();
            giaodich.J_KHACHHANG = J_HOSOKHACHHANG;
            giaodich.NGAYGIAODICH = DateTime.Parse(NGAYGIAODICH);
            giaodich.ID_DTTC = clsFunctions.GetUserID();
            giaodich.ID_TINHTRANGGD = ID_TINHTRANGGD;
            //giaodich.ID_TIEMNANG = ID_TIEMNANG;
            giaodich.NOIDUNG_GIAODICH = NOIDUNG_GIAODICH;
            giaodich.CHOTCACDIEM_THONGNHAT = CHOTCACDIEMTHONGNHAT;
            db.TH_GIAODICH.Add(giaodich);
            return Json(db.SaveChanges(), JsonRequestBehavior.AllowGet);
        }


    }
}