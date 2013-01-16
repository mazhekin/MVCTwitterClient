using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Core;
using TweetSharp;

namespace App.Web.Controllers
{
    public class SearchController : BaseController
    {
        private int pageSize = 15;

        public SearchController() : base()
        {
        }

        //
        // GET: /Search/

        public ActionResult Index(string q)
        {
            ViewBag.q = q;
            ViewBag.ShowMore = true;

            if (!String.IsNullOrWhiteSpace(q))
            {
                ViewBag.TwitterSearchResult = this.contentService.Search(q, 1 , this.pageSize);
            }

            return View();
        }

        //
        // GET: /Search/Tweets/

        public ActionResult Tweets(string q, int page)
        {
            var searchResult = this.contentService.Search(q, page, this.pageSize);
            return PartialView("_TwitterSearchStatuses", searchResult.Statuses);
        }

        //
        // GET: /Search/Users

        public ActionResult Users(string q)
        {
            ViewBag.q = q;
            ViewBag.ShowMore = true;

            if (!String.IsNullOrWhiteSpace(q))
            {
                var users = this.contentService.SearchForUsers(this.GetAccessToken(), q, 1, this.pageSize);
                ViewBag.TwitterUsers = users;
            }

            return View();
        }
    }
}
