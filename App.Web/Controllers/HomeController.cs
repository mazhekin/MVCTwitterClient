using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Core;
using App.Web.Models;
using TweetSharp;

namespace App.Web.Controllers
{
    public class HomeController : BaseController
    {
        private int pageSize = 15;

        public HomeController() : base()
        {
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var tweets = this.contentService.GetTweets(this.GetAccessToken(), 1, this.pageSize);
            return View(tweets);
        }

        //
        // GET: /Home/Tweets/

        public ActionResult Tweets(int page)
        {
            var tweets = this.contentService.GetTweets(this.GetAccessToken(), page, this.pageSize);
            return PartialView("_TwitterStatuses", tweets);
        }

        //
        // GET: /Home/Login

        public ActionResult Login()
        {
            Session["RequestToken"] = this.contentService.GetRequestTokenXml();
            ViewBag.AuthUrl = this.contentService.GetAuthUrl(Session["RequestToken"] as String);
            return View();
        }

        //
        // POST: /Home/Login

        [HttpPost]
        public ActionResult Login(string pin)
        {
            if (String.IsNullOrWhiteSpace(pin))
            {
                ModelState.AddModelError("pin", "Pin value cannot be empty.");
            }
            else
            {
                var accessToken = this.contentService.GetAccessToken(Session["RequestToken"] as String, pin);

                if (accessToken == null)
                {
                    ModelState.AddModelError("pin", "Pin value is incorrect or expired.");
                }

                if (ModelState.IsValid)
                {
                    var accessTokenXml = (accessToken as Object).Serialize<OAuthAccessToken>();
                    formsAuth.SignIn(accessTokenXml, false);

                    return RedirectToAction("Index");
                }
            }

            return Login();
        }

        //
        // GET: /Home/Logout

        public ActionResult Logout(string @return)
        {
            formsAuth.SignOut();
            if (String.IsNullOrWhiteSpace(@return))
            {
                return RedirectToAction("index", "home");
            }
            return Redirect(@return);
        }
    }
}
