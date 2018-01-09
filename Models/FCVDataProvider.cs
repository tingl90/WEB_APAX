
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Web_Apax.Models;

namespace Web_Apax.Models
{
    public class FCVDataProvider
    {
        public static string GetUserName()
        {
            try { return HttpContext.Current.User.Identity.Name; }
            catch { return null; }
        }
        public static int GetUserID(string userName)
        {
            using (APAXEntities3 db = new APAXEntities3())
                try { return Task.Run(() => 
                   db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(userName)).ID_DTTC).Result;
                }
                catch { return 0; }
        }        
    }
}