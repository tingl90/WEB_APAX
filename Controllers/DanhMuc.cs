using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using Web_Apax.Models;
using System.Linq;
using System;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DanhMucController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetTrungTam()
        {
            try
            {
                return Json(db.DM_TRUNGTAM.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCM()
        {
            try
            {
                return Json(db.DM_DTTC.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetChuongTrinhSanPham()
        {
            try
            {
                return Json(db.TH_SANPHAM.Where(t=>t.ID_LOAISANPHAM == 138055).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetChuongTrinh()
        {
            return Json(db.DM_LOAIKEHOACH.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTrangThaiHoc()
        {
            try
            {
                return Json(db.DM_LOAIHOPDONG.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        #region Tuấn
        public JsonResult DM_LYDOBAOLUU()
        {
            return Json(db.DM_LYDONT.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPhuongThuc()
        {
            return Json(db.DM_TINHTRANGGD.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTiemNang()
        {
            return Json(db.DM_TIEMNANG.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DM_TinhThanhPho()
        {
            return Json(db.DM_TINHTHANHPHO.OrderBy(it=>it.TENTINHTHANHPHO).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DM_QuanHuyen(int Tinh)
        {
            if (Tinh == 0)
            {
                return Json(db.DM_QUANHUYEN.OrderBy(it=>it.TENQUANHUYEN).ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(db.DM_QUANHUYEN.Where(x => x.JD_TINHTHANHPHO == Tinh).OrderBy(it => it.TENQUANHUYEN).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DM_LOAIKHACHHANG()
        {
            return Json(db.DM_LOAIKHACHHANG.ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DM_GIOITINH()
        {
            return Json(db.DM_GIOITINH.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Dũng
        public JsonResult DM_DuyetThucHien()
        {
            return Json(db.DM_DUYET.ToList(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}