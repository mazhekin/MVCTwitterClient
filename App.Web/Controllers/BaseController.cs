using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TweetSharp;
using App.Core;
using App.Web.Models;

namespace App.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IContentService contentService;
        protected readonly IFormsAuthentication formsAuth;

        public BaseController()
        {
            this.contentService = (IContentService)MvcApplication.Container.Resolve(typeof(IContentService), null);
            this.formsAuth = (IFormsAuthentication)MvcApplication.Container.Resolve(typeof(IFormsAuthentication), null);
        }

        protected OAuthAccessToken GetAccessToken()
        {
            var accessToken = this.contentService.GetAccessToken(this.HttpContext.User.Identity.Name);
            if (accessToken == null)
            {
                formsAuth.SignOut();
            }
            return accessToken;
        }
    }
}
