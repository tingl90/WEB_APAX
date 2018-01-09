//using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Web_Apax.Models
{
    public class BSCFunc_Q
    {
        public static TempDataDictionary tmp = new TempDataDictionary();
        public static string passDefault = "abc@123";
        public static string keyPass = "BSC SOFT";
        public static string defaultLanguage = "vi-VN";
        public static List<GVSettings> ListGVSettings = new List<GVSettings>();
        public partial class GVSettings
        {
            public string Username { get; set; }
            //public GridViewSettings Settings { get; set; }
        }
        #region STRING
        public static string GetUserName()
        {
            var userName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            return userName;
        }
        public static string TachHoTen(string str)
        {
            string res = "";
            string[] tu = str.Split(' ');
            int len = tu.Length;
            for (int i = 0; i < len - 1; i++)
            {
                res += tu[i].Substring(0, 1);
            }
            res += tu[len - 1];
            return res;
        }
        #endregion

        #region INT
        public async Task<int> GetUserID(HttpCookieCollection Cookies)
        {
            try
            {
                using (APAXEntities3 db = new APAXEntities3())
                {
                    string userName = GetUserName().ToLower();
                    TH_HOSOKHACHHANG model = await Task.Run(()=> db.TH_HOSOKHACHHANG
                        .FirstOrDefault(it => it.TAIKHOAN.ToLower().Equals(userName)));
                    if (model != null)
                        return model.A_HOSOKHACHHANG;
                    else
                        return 1;
                }
            }
            catch { return 1; }
        }
        public async Task<TH_HOSOKHACHHANG> GetHoSoKhachHang(int? ID)
        {
            try
            {
                using (APAXEntities3 db = new APAXEntities3())
                {
                    if (ID>0)
                    {
                        TH_HOSOKHACHHANG model = await Task.Run(()=>db.TH_HOSOKHACHHANG.FirstOrDefault(it => it.A_HOSOKHACHHANG==ID));
                        return model;
                    }
                    return null;
                }
            }
            catch { return null; }
        }
        public async Task<TH_DUBAO> GetDKNghiPhep(int? ID)
        {
            try
            {
                using (APAXEntities3 db = new APAXEntities3())
                {
                    if (ID > 0)
                    {
                        TH_DUBAO model = await Task.Run(() => db.TH_DUBAO.FirstOrDefault(it => it.A_TH_DUBAO == ID));
                        return model;
                    }
                    return null;
                }
            }
            catch { return null; }
        }
        #endregion

        #region BOOL
        public bool SendToMail(Q_SendMail host, string email, string subject, string body)
        {
            try
            {
                if (host != null)
                {
                    var message = new System.Net.Mail.MailMessage();
                    message.To.Add(new System.Net.Mail.MailAddress(email));
                    message.From = new System.Net.Mail.MailAddress(host.Email, ResourceManager.GetString("CompanyName"));
                    message.BodyEncoding = System.Text.UTF8Encoding.UTF8;
                    message.SubjectEncoding = System.Text.UTF8Encoding.UTF8;
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    using (System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient())
                    {
                        client.UseDefaultCredentials = true;
                        client.Credentials = new System.Net.NetworkCredential(host.Email, clsSecurity.Decrypt(host.MatKhau, keyPass));
                        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                        client.Host = host.SMTPServer;
                        client.Port = host.Port;
                        client.EnableSsl = true;
                        client.Send(message);
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }
        public static bool CheckUserRole(string role)
        {
            try
            {
                string[] spRole = role.Split(',');
                if (spRole != null && spRole.Count() > 0)
                {
                    foreach (string r in spRole)
                    {
                        bool rs = System.Threading.Thread.CurrentPrincipal.IsInRole(r);
                        if (rs)
                            return rs;
                    }
                }
                return false;
            }
            catch { return false; }
        }
        #endregion
        #region EXPORT XLS
       // public static GridViewSettings GridExportXLS { get { return ListGVSettings.FirstOrDefault(it => it.Username.Equals(HttpContext.Current.User.Identity.Name)).Settings; } }
        //public static void SetGridExportXLS(GridViewSettings gridSetting)
        //{
        //    GVSettings gvs = new GVSettings();
        //    gvs.Username = HttpContext.Current.User.Identity.Name;
        //    gvs.Settings = gridSetting;
        //    var modelItem = ListGVSettings.FirstOrDefault(it => it.Username.Equals(HttpContext.Current.User.Identity.Name));
        //    if (modelItem != null)
        //        ListGVSettings.Remove(modelItem);
        //    ListGVSettings.Add(gvs);
        //}
        #endregion
    }
}