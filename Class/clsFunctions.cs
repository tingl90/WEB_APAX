using System.Linq;

namespace Web_Apax.Models
{
    public class clsFunctions
    {
        public static string GetUsername()
        {
            return System.Threading.Thread.CurrentPrincipal.Identity.Name.Trim();
        }
        public static int GetUserID()
        {
            var name = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            using (APAXEntities3 db = new APAXEntities3())
            {
                return (db.DM_DTTC.FirstOrDefault(it => it.TAIKHOAN.Equals(name))).ID_DTTC;
            }
        }
    }
}