using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Apax.Models
{
    [RoleAuthorize]
    public class LanguageController : Controller
    {
        public ActionResult ChangeLanguage(string KeyLanguage, string returnUrl)
        {
            if (!string.IsNullOrEmpty(KeyLanguage))
            {
                var httpCookie = Request.Cookies["language"];
                if (httpCookie != null)
                {
                    var cookie = Response.Cookies["language"];
                    if (cookie != null)
                        cookie.Value = KeyLanguage;
                }
            }
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }
	}
}