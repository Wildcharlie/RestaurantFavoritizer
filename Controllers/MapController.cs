using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantFavoritizer.DAL;
using RestaurantFavoritizer.Models;

namespace RestaurantFavoritizer.Controllers
{
    public class Mapcontroller : Controller
    {
        private FavContext db = new FavContext();

        [Authorize]
        public ActionResult Index()
        {
            FavUser user;
            MainMapViewClass cls;

            user = FavUserMaster.GetCurrentUser(db);

            var restaurants = from r in db.Restaurants
                              where r.MainLocation.City == user.Location.City
                              select r;

            try
            {
                cls = new MainMapViewClass { Restaurants = restaurants.ToArray(), FavUser = user };
            }
            catch
            {
                cls = new MainMapViewClass { Restaurants = null, FavUser = user };
            }
            return View(cls);
        }

        // GET: Mapcontroller/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavUser FavUser = db.FavUsers.Find(id);
            if (FavUser == null)
            {
                return HttpNotFound();
            }
            return View(FavUser);
        }

        // GET: Mapcontroller/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mapcontroller/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID")] FavUser FavUser)
        {
            if (ModelState.IsValid)
            {
                db.FavUsers.Add(FavUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(FavUser);
        }

        // GET: Mapcontroller/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavUser FavUser = db.FavUsers.Find(id);
            if (FavUser == null)
            {
                return HttpNotFound();
            }
            return View(FavUser);
        }

        // POST: Mapcontroller/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID")] FavUser FavUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FavUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FavUser);
        }

        // GET: Mapcontroller/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FavUser FavUser = db.FavUsers.Find(id);
            if (FavUser == null)
            {
                return HttpNotFound();
            }
            return View(FavUser);
        }

        // POST: Mapcontroller/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FavUser FavUser = db.FavUsers.Find(id);
            db.FavUsers.Remove(FavUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult LocalFavorites(float neLat, float neLong, float swLat, float swLong, int? srchType, string srchTerm)
        {
            var rests = new List<object>();
            FavUser user;
            RestaurantType type;

            if (srchType != null)
            {
                type = (RestaurantType)srchType;
            }
            else
            {
                type = RestaurantType.Other;
            }

            user = FavUserMaster.GetCurrentUser(db);

            var restaurants = from r in db.Restaurants
                              where r.MainLocation.City == user.Location.City
                              && r.MainLocation.Latitude <= neLat
                              && r.MainLocation.Latitude >= swLat
                              && r.MainLocation.Longitude <= neLong
                              && r.MainLocation.Longitude >= swLong
                              && (srchType == null || r.RestaurantType == type)
                              && (srchTerm == "undefined" || r.RestaurantName.Contains(srchTerm))
                              select r;

            var _counts = db.Favorites.Where(m => m.Restaurant.MainLocation.City == user.Location.City)
                .GroupBy(m => m.RestaurantID).ToDictionary(d => d.Key, d => d.Count());

            foreach (Restaurant r in restaurants.ToArray())
            {
                r.FavCount = (_counts.ContainsKey(r.RestaurantID)) ? _counts[r.RestaurantID] : 0;

                rests.Add(new
                {
                    id = r.RestaurantID,
                    IWString = FormatInfoWindow(r),
                    PlaceName = r.RestaurantName,
                    vicinity = (r.MainLocation.AddressLn1 == null ? "" : r.MainLocation.AddressLn1),
                    phone = (r.MainLocation.PhoneNo == null ? "" : r.MainLocation.PhoneNo),
                    GeoLong = r.MainLocation.Longitude,
                    GeoLat = r.MainLocation.Latitude,
                    IconName = r.IconName,
                    favcount = r.FavCount,
                    rtype = EnumNinja.DisplayName(r.RestaurantType),
                    website = (r.WebSiteURL == null ? "" : r.WebSiteURL)
                });
            }

            return Json(rests, JsonRequestBehavior.AllowGet);
        }

        private string FormatInfoWindow(Restaurant rest)
        {
            return rest.MainLocation.ContentString;
        }
    }
}