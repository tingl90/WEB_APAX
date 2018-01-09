using System.Linq;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class DanhSachLopController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public string TaiKhoan = clsFunctions.GetUsername();
        // GET: DanhSachLop
        public ActionResult DanhSachLopHocChiTiet()
        {
            return View();
        }
        public ActionResult ChiTietLopHoc(int? A_KEHOACH)
        {
            return View(db.SP_APAX_DANHSACHLOP(0,0, "Administrator").FirstOrDefault(it => it.A_KEHOACH == A_KEHOACH));
        }
        public JsonResult GetDanhSachLopHoc()
        {
            var model = db.APAX_TIMKIEM_LOPHOC("", "", TaiKhoan).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
    }
}