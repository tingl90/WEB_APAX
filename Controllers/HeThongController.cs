using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data;
using System.Web.UI.WebControls;
using Web_Apax.Models;
using System.Collections.Generic;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class HeThongController : Controller
    {
        // GET: HeThong
        APAXEntities3 db = new APAXEntities3();
        public ActionResult PhanQuyen()
        {
            return View();
        }
        public async Task<JsonResult> Get_DanhSachNhanSu(string ma, string trungtam)
        {
            try
            {
                ma = !String.IsNullOrEmpty(ma) ? ma : "";
                trungtam = !String.IsNullOrEmpty(trungtam) && trungtam != "null" ? trungtam : "";
                var model = await Task.Run(() => db.APAX_DANHSACH_NHANSU(ma, trungtam).ToList());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Get_DanhSachNhanSu_Leader( string trungtam)
        {
            try
            {
                trungtam = !String.IsNullOrEmpty(trungtam) && trungtam != "null" ? trungtam : "";
                var model = await Task.Run(() => db.APAX_DANHSACH_NHANSU_leader( trungtam).ToList());
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Get_CB_TrungTam()
        {
            try
            {
                var model = await db.DM_TRUNGTAM.ToListAsync();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
        public async Task<JsonResult> Save_PhanQuyen(string ma, string quyen)
        {
            try
            {
                var model = await db.DM_DTTC.FirstOrDefaultAsync(it => it.MADTTC.Equals(ma.Trim()));
                if (model != null)
                {
                    model.MANGUOIQUANLY = quyen;
                    db.Entry(model).State = EntityState.Modified;
                }
                return Json(await db.SaveChangesAsync(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        //public JsonResult Get_Tree()
        //{
        //    try
        //    {
        //        var data = new List<TreeQuyen>();
        //        data.Add(new TreeQuyen
        //        {
        //            id = "1",
        //            text = "APAX",
        //            items = Get_Vung()
        //        });
        //        return Json(data, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new string[] { ex.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public List<TreeQuyen> Get_Vung()
        //{
        //    var tt = db.DM_VUNG.ToList();
        //    var model = Task.Run(() =>
        //              (from it in tt
        //               select new TreeQuyen
        //               {
        //                   id = "1_" + it.ID_VUNG,
        //                   text = it.TENVUNG,
        //                   items = Get_TrungTam(it.MAVUNG, it.ID_VUNG)
        //               }).ToList()).Result;
        //    return model != null && model.Count > 0 ? model : null;
        //}
        //public List<TreeQuyen> Get_TrungTam(string vung, int vu)
        //{
        //    var tt = db.DM_TRUNGTAM.Where(it => it.MA_VUNG.Equals(vung)).ToList();
        //    var model = Task.Run(() =>
        //              (from it in tt
        //               select new TreeQuyen
        //               {
        //                   id = "1_" + vu + "_" + it.ID_TRUNGTAM,
        //                   text = it.TENTRUNGTAM,
        //                   items = Get_Leader(it.MA_DVCS, vu, it.ID_TRUNGTAM)
        //               }).ToList()).Result;
        //    return model != null && model.Count > 0 ? model : null;
        //}
        //public List<TreeQuyen> Get_Leader(string matt, int vu, int tt)
        //{
        //    var ld = db.DM_DTTC.Where(it => it.MATRUNGTAM.Equals(matt) && (it.JD_BOPHAN == 180 || it.JD_BOPHAN == 185)).ToList();
        //    var model = Task.Run(() =>
        //               (from it in ld
        //                select new TreeQuyen
        //                {
        //                    id = "1_" + vu + "_" + tt + "_" + it.ID_DTTC,
        //                    text = it.TENDTTC,
        //                    items = Get_CM_Sale(it.MADTTC, vu, tt, it.ID_DTTC)
        //                }).ToList()).Result;
        //    return model != null && model.Count > 0 ? model : null;
        //}
        //public List<TreeQuyen> Get_CM_Sale(string macm, int vu, int tt, int dt)
        //{
        //    var ld = db.DM_DTTC.Where(it => it.MANGUOIQUANLY.Equals(macm) && (it.JD_BOPHAN == 182 || it.JD_BOPHAN == 183)).ToList();
        //    var model = Task.Run(() =>
        //              (from it in ld
        //               select new TreeQuyen
        //               {
        //                   id = "1_" + vu + "_" + tt + "_" + dt + "_" + it.ID_DTTC,
        //                   text = it.TENDTTC
        //               }).ToList()).Result;
        //    return model != null && model.Count > 0 ? model : null;
        //}
        //public async Task<JsonResult> Save_PhanQuyen(string ma,string quyen)
        //{
        //    try
        //    {
        //        var item = quyen.Remove(quyen.LastIndexOf(",")).Replace("Pull down to refresh...Release to refresh...Refreshing...","").Split(',');
        //        var model = await db.DM_DTTC.FirstOrDefaultAsync(it => it.MADTTC.Equals(ma.Trim()));
        //        if (model != null)
        //        {
        //            foreach(string it in item)
        //            {
        //                if(it.Length - it.Replace("_","").Length == 4)
        //                {
        //                    model.qEC += it + ",";// it.Substring(it.LastIndexOf("_"), it.Length - it.LastIndexOf("_")) + ',';
        //                }else
        //                {
        //                    if (it.Length - it.Replace("_", "").Length == 3)
        //                    {
        //                        model.qLD += it + ",";//it.Substring(it.LastIndexOf("_"), it.Length - it.LastIndexOf("_")) + ',';
        //                    }
        //                    else
        //                    {
        //                        if (it.Length - it.Replace("_", "").Length == 2)
        //                        {
        //                            model.qTT += it + ",";//it.Substring(it.LastIndexOf("_"), it.Length - it.LastIndexOf("_")) + ',';
        //                        }
        //                        else
        //                        {
        //                            model.qV += it + ",";//it.Substring(it.LastIndexOf("_"), it.Length - it.LastIndexOf("_")) + ',';
        //                        }
        //                    }
        //                }
        //            }
        //            db.Entry(model).State = EntityState.Modified;
        //        }
        //        return Json(await db.SaveChangesAsync(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
        //    }
        //}
        //public async Task<JsonResult> Get_Quyen(string ma)
        //{
        //    var model = await db.DM_DTTC.FirstOrDefaultAsync(it => it.MADTTC.Equals(ma.Trim()));
        //    //var ld = model.qLD != null ? model.qLD.Split(',') : null;
        //    //var tt = model.qLD != null ? model.qLD.Split(',') : null;
        //    //var v = model.qLD != null ? model.qLD.Split(',') : null;
        //    //var quyen = v.Count() > 0 ? "1_" + model.qV.Replace(",", ",1_") : "";
        //    var quyen = model.qV + model.qTT + model.qLD + model.qEC;
        //    return Json(quyen, JsonRequestBehavior.AllowGet);
        //}
        //public partial class TreeQuyen
        //{
        //    public string id { get; set; }
        //    public string text { get; set; }
        //    public List<TreeQuyen> items { get; set; }
        //}
    }
}