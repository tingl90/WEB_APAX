using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Web_Apax.Models;

namespace Web_Apax.Controllers
{
    [RoleAuthorize]
    public class AccountController : Controller
    {
        APAXEntities3 db = new APAXEntities3();
        BSCFunc fn = new BSCFunc();

        #region Login
        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public ActionResult Login(AccountModel model, string returnUrl)
        {
            //if (ModelState.IsValid)
            //{                
            //string MatKhau = EncryptAndDecrypt.Encrypt(model.Password, "BSC SOFT");
            string MatKhau = model.Password;
            var nguoidung = db.DM_DTTC.Where(m => m.TAIKHOAN == model.UserName && m.MATKHAU == MatKhau).FirstOrDefault();
            if (nguoidung != null)
            {
                FormsAuthentication.SetAuthCookie(nguoidung.TAIKHOAN, model.RememberMe);

                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {

                    return RedirectToAction("QuanLyHocVien", "HoSoHocVien");
                }
            }

            else
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mất khẩu.");
            }
            return View("Login", model);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        #endregion
    }
}