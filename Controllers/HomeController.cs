using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Data.Entity;
using System.Drawing;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        public ActionResult Index()
        {
            try
            {
                string scheme = System.Web.HttpContext.Current.Request.Url.Host;
                if (!scheme.Equals("localhost"))
                    db.Database.ExecuteSqlCommand("INSERT INTO HT_History(ip,USERs,ULR)values(N'" + System.Web.HttpContext.Current.Request.UserHostAddress + "',N'" + User.Identity.Name + "',N'" + scheme + "')");
            }
            catch
            {

            }
            return View();
        }               

       
    }
}