using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantFavoritizer.Models;
using RestaurantFavoritizer.DAL;
using System.Linq.Expressions;
using System.Web.Mvc.Html;

namespace RestaurantFavoritizer.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private FavContext db = new FavContext();

        [Authorize]
        public ActionResult Index()
        {
            return View(RestaurantMaster.GetMainViewClass(db));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Adds a new restaurant to the database
        /// </summary>
        /// <param name="rest"> the Restaurant object that is to be saved to db.</param>
        /// <param name="loc">location object that will be the restaurant main location.</param>
        /// <returns>a message indicating success or failure</returns>
        [HttpPost]
        public ActionResult AddNewRestaurant(Restaurant rest, Location loc)
        {
            if (ModelState.IsValid)
            {
                FavUser user;
                Favorite fav;
                Perk perk;
                
                if(rest.WebSiteURL != null && rest.WebSiteURL.Trim() != String.Empty)
                {
                    if(!rest.WebSiteURL.ToLower().StartsWith("http"))
                    {
                        rest.WebSiteURL = "http://" + rest.WebSiteURL;
                    }
                }

                user = FavUserMaster.GetCurrentUser(db);
                perk = new Perk { };
                fav = new Favorite { };
                rest.IconName = "restaurant";
                loc.City = user.Location.City;
                loc.State = user.Location.State;
                fav.CreateDate = DateTime.Now;
                fav.FavUserID = user.FavUserID;
                fav.RestaurantID = rest.RestaurantID;
                rest.MainLocation = loc;
                rest.CreateDate = DateTime.Now;
                perk.PerkType = PerkType.AddedRestaurant;
                perk.FavUserID = user.FavUserID;
                perk.Value = 2;
                perk.CreateDate = DateTime.Now;
                db.FavUsers.Attach(user);
                user.PerkTotal = user.PerkTotal + perk.Value;

                db.Favorites.Add(fav);
                db.Locations.Add(loc);
                db.Restaurants.Add(rest);
                db.Perks.Add(perk);

                try
                {
                    db.SaveChanges();
                    return PartialView(rest);
                }
                catch
                {
                }
            }
            return null;
        }

        public ViewResult Search(String searchString)
        {
            MainViewClass RestaurantView;
            FavUser user;
            db.Configuration.ProxyCreationEnabled = false;

            user = FavUserMaster.GetCurrentUser(db);

            var rests = from m in db.Restaurants
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                rests = rests.Where(s => s.RestaurantName.Contains(searchString));
            }

            rests = rests.OrderBy(s => s.RestaurantName);

            try
            {
                RestaurantView = new MainViewClass { Restaurants = rests.ToArray(), FavUser = user };
            }
            catch
            {
                RestaurantView = new MainViewClass { Restaurants = new List<Restaurant> { }, FavUser = user };
            }

            return View(RestaurantView);
        }

    }
}