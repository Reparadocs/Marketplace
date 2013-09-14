using System;
using System.Collections.Generic;

using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using EIMarketplace.Models;
using EIMarketplace.Filters;

namespace EIMarketplace.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private ListingDBContext db = new ListingDBContext();


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Search", "Listing");/*
                var listings = from m in db.Listings
                             select m;
                var userID = WebSecurity.GetUserId(User.Identity.Name);
                listings = listings.Where(m => m.CreatorID == userID);

                foreach (Listing l in listings)
                {
                    l.Title = TruncateTD(l.Title);
                    l.Description = TruncateTD(l.Description);
                }

                return View(listings);
              */
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
          
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string TruncateTD(string longString)
        {

            int limit = 50;
            if (longString.Length > limit)
                return longString.Substring(0, limit) + "...";

            return longString;
        }
    }
    

}
