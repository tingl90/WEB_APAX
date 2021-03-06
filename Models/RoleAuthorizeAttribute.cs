﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Apax.Models
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private string redirectUrl = "";
        public RoleAuthorizeAttribute()
            : base()
        {
        }

        public RoleAuthorizeAttribute(string redirectUrl)
            : base()
        {
            this.redirectUrl = redirectUrl;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                string authUrl = this.redirectUrl; //passed from attribute

                //if null, get it from config
                if (String.IsNullOrEmpty(authUrl))
                    authUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["RolesAuthRedirectUrl"];

                if (!String.IsNullOrEmpty(authUrl))
                    filterContext.HttpContext.Response.Redirect(authUrl);
            }

            //else do normal process
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}