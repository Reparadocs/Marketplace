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
    public class ListingController : Controller
    {
        private ListingDBContext db = new ListingDBContext();

        //
        // GET: /Listing/

        public ActionResult Index()
        {
            return View(db.Listings.ToList());
        }


        public ActionResult Search(string searchString)
        {
            var listings = from m in db.Listings
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                listings = listings.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString));
            }

            return View(listings);
        }

        
        //
        // GET: /Listing/Details/5

        public ActionResult Details(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        //
        // GET: /Listing/Create

        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("LogIn", "Home");
        }

        //
        // POST: /Listing/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Listing listing)
        {
            if (listing.AutoAcceptMax == null)
            {
                listing.AutoAcceptMax = 0;
            }

            if (listing.ExpirationDate == null)
            {
                listing.ExpirationDate = DateTime.MaxValue;
            }

            listing.LastActivity = DateTime.Today;

            if (ModelState.IsValid)
            {
                listing.Status = ListingStatus.Created;
                listing.CreatorID = WebSecurity.GetUserId(User.Identity.Name);
                db.Listings.Add(listing);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(listing);
        }

        //
        // GET: /Listing/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        //
        // POST: /Listing/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Listing listing)
        {
            if (listing.AutoAcceptMax == null)
            {
                listing.AutoAcceptMax = 0;
            }

            if (listing.ExpirationDate == null)
            {
                listing.ExpirationDate = DateTime.MaxValue;
            }

            listing.LastActivity = DateTime.Today;

            if (ModelState.IsValid)
            {
                
                listing.Status = ListingStatus.Created;
                listing.CreatorID = WebSecurity.GetUserId(User.Identity.Name);
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(listing);
        }

        //
        // GET: /Listing/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        //
        // POST: /Listing/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Listing listing = db.Listings.Find(id);
            db.Listings.Remove(listing);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}