using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    public class BaoCaoController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public ActionResult HocSinhMoi()
        {
            return View();
        }
        public JsonResult Get_HocSinhMoi(string tungay, string denngay)
        {
            var tn = DateTime.Parse(tungay);
            var dn = DateTime.Parse(denngay);
            var model = db.BAOCAO_APAX_DS_HOCSINHMOI(tn,dn).ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult HocSinh()
        {
            return View();
        }
        public JsonResult Get_HocSinh()
        {
            var model = db.BAOCAO_APAX_DS_HOCSINH().ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }

        public ActionResult ChoXepLop()
        {
            return View();
        }
        public JsonResult Get_ChoXepLop()
        {
            var model = db.BAOCAO_APAX_DS_CHOXEPLOP().ToList();
            var json = Json(model, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = int.MaxValue;
            return json;
        }
    }
}