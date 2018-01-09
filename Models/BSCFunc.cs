using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.IO;
using System.Web.Mvc;

namespace Web_Apax.Models
{
    public class BSCFunc
    {
        public static TempDataDictionary tmp = new TempDataDictionary();
        public static string passDefault = "abc@123";
        public static string keyPass = "BSC SOFT";
        public static string defaultLanguage = "vi-VN";
        #region BOOL
        public static bool CheckUserRole(string role)
        {
            try
            {
                string[] spRole = role.Split(',');
                foreach (string r in spRole)
                {
                    if (System.Threading.Thread.CurrentPrincipal.IsInRole(r))
                        return true;
                }
                return false;
            }
            catch { return false; }
        }
        #endregion

        #region Data<T>
        public async Task<PQ_NGUOIDUNG> GetUserLogin(string userName, string pass)
        {
            try
            {
                using (APAXEntities3 db = new APAXEntities3())
                {
                    if (!String.IsNullOrEmpty(userName))
                    {
                        PQ_NGUOIDUNG model = await db.PQ_NGUOIDUNG.FirstOrDefaultAsync(it => it.TAIKHOAN.Equals(userName) && it.MATKHAU.Equals(pass));
                        return model;
                    }
                    return null;
                }
            }
            catch { return null; }
        }
        #endregion

        #region Data<Int>
        public static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }
        #endregion
    }
}