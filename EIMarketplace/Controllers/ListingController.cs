using System;
using System.Collections.Generic;
using PagedList;
using PagedList.Mvc;
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
            if (User.Identity.IsAuthenticated)
            {
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
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }


        public ActionResult Search(string searchString, string button, int? page)
        {
            int pageSize = 5;
            var listings = from m in db.Listings
                           select m;

          

            if (!String.IsNullOrEmpty(searchString))
            {
                page = 1;
                var searchWords = searchString.Split(' ');
                listings = listings.Where(s => searchWords.All(t => s.Title.Contains(t) || s.Description.Contains(t)));
                //listings = listings.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString));
            }

            if (button != null)
            {
                switch (button)
                {
                    case "SearchFreelancers":
                        listings = listings.Where(s => s.Type == ListingType.Freelancer);
                        break;
                    case "SearchStartups":
                        listings = listings.Where(s => s.Type == ListingType.Startup);
                        break;
                    default:
                        break;
                }
            }

            foreach (Listing l in listings)
            {
                l.Title = TruncateTD(l.Title);
                l.Description = TruncateTD(l.Description);
            }

            IOrderedQueryable<Listing> orderedListings;
            orderedListings = listings.OrderByDescending(s => s.Title);

            int pageNumber = (page ?? 1);
            return View(orderedListings.ToPagedList(pageNumber, pageSize));
        }

        
        //
        // GET: /Listing/Details/5

        public ActionResult ODetails(int id = 0)
        {
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }

            if (listing.CreatorID == WebSecurity.GetUserId(User.Identity.Name))
            {
                return View(listing);
            }
            else
            {
                return HttpNotFound();
            }
        }

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

        public ActionResult CreateFreelancer()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        public ActionResult CreateStartup()
        {
            if (User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Login", "Account");
        }

        //
        // POST: /Listing/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFreelancer(Listing listing)
        {
            /*
            if (listing.AutoAcceptMax == null)
            {
                listing.AutoAcceptMax = 0;
            }

            if (listing.ExpirationDate == null)
            {
                listing.ExpirationDate = DateTime.MaxValue;
            }

            listing.LastActivity = DateTime.Today;
            */
            if (ModelState.IsValid)
            {
                listing.Status = ListingStatus.Created;
                listing.Type = ListingType.Freelancer;
                 
                /*
                listing.CreatorID = WebSecurity.GetUserId(User.Identity.Name);
                 */
                using (var dbContext = new UsersContext())
                {
                    var user = dbContext.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
                    listing.CreatorID = user.UserId;
                    listing.CreatorName = user.Name;
                    listing.CreatorContact = user.Email;
                    
                }
                db.Listings.Add(listing);
                db.SaveChanges();
                return RedirectToAction("Search", "Listing");
            }
            return View(listing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStartup(Listing listing)
        {
            /*
            if (listing.AutoAcceptMax == null)
            {
                listing.AutoAcceptMax = 0;
            }

            if (listing.ExpirationDate == null)
            {
                listing.ExpirationDate = DateTime.MaxValue;
            }

            listing.LastActivity = DateTime.Today;
            */
            if (ModelState.IsValid)
            {
                listing.Status = ListingStatus.Created;
                listing.Type = ListingType.Startup;

                /*
                listing.CreatorID = WebSecurity.GetUserId(User.Identity.Name);
                 */
                using (var dbContext = new UsersContext())
                {
                    var user = dbContext.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
                    listing.CreatorID = user.UserId;
                    listing.CreatorName = user.Name;
                    listing.CreatorContact = user.Email;
                    
                }
                db.Listings.Add(listing);
                db.SaveChanges();
                return RedirectToAction("Search", "Listing");
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

            if (listing.CreatorID == WebSecurity.GetUserId(User.Identity.Name))
            {
                return View(listing);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //
        // POST: /Listing/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Listing listing)
        {
            /*
            if (listing.AutoAcceptMax == null)
            {
                listing.AutoAcceptMax = 0;
            }

            if (listing.ExpirationDate == null)
            {
                listing.ExpirationDate = DateTime.MaxValue;
            }

            listing.LastActivity = DateTime.Today;
            */
            if (ModelState.IsValid)
            {
                
                listing.Status = ListingStatus.Created;
                listing.CreatorID = WebSecurity.GetUserId(User.Identity.Name);
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Search", "Listing");
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

            if (listing.CreatorID == WebSecurity.GetUserId(User.Identity.Name))
            {
                return View(listing);
            }
            else
            {
                return HttpNotFound();
            }
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
            return RedirectToAction("Search", "Listing");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
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