using System;
using System.Collections.Generic;
using System;
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

        public ActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Log In" + User.Identity.Name;
                return View();
            }
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var listings = from m in db.Listings
                             select m;
                var userID = WebSecurity.GetUserId(User.Identity.Name);
                listings = listings.Where(m => m.CreatorID == userID);

                return View(listings);
            }
            else
            {
                return RedirectToAction("LogIn", "Home");
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
    }
}
