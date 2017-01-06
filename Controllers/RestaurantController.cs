using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RestaurantFavoritizer.Models;
using RestaurantFavoritizer.DAL;

namespace RestaurantFavoritizer.Controllers
{
    public class RestaurantController : Controller
    {
        private FavContext db = new FavContext();

        // GET: Restaurant
        public ActionResult Index()
        {
            return View();
        }

        // GET: Restaurant/Details/5b v 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RestaurantViewClass RestaurantView;
            FavUser user;

            user = FavUserMaster.GetCurrentUser(db);
            //Restaurant restaurant = db.Restaurants.Find(id);
            Restaurant restaurant = RestaurantMaster.GetRestaurantByID(id,db);

            if (restaurant == null)
            {
                return HttpNotFound();
            }

            try
            {
                RestaurantView = new RestaurantViewClass { Restaurant = restaurant, FavUser = user };
                return View(RestaurantView);
            }
            catch
            {
                return HttpNotFound();
            }
        }

        public ActionResult Place(string placeID, string name, string phone, string address, float lat, float lng, string url, string site)
        {
            if (placeID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = db.Restaurants.FirstOrDefault(Restaurant => Restaurant.MainLocation.GooglePlaceID == placeID);
            if (restaurant == null)
            {
                FavUser user;
                Location location;
                //Favorite fav;
                Perk perk;
                string website;
                perk = new Perk { };
                //fav = new Favorite { };
                user = FavUserMaster.GetCurrentUser(db);
                location = new Location { GooglePlaceID = placeID };

                if(site != null && site.Trim() != String.Empty)
                {
                    if (!site.StartsWith("http"))
                    {
                        website = "https://" + site;
                    }
                    else
                    {
                        website = site;
                    }
                }
                else
                {
                    website = string.Empty;
                }


                location.Latitude = lat;
                location.Longitude = lng;
                location.AddressLn1 = address;
                location.PhoneNo = phone;
                location.GooglePlaceID = placeID;
                location.ContentString = "Be the first to add a description and earn fav points!";
                location.Annotation = url;
                location.City = user.Location.City;
                restaurant = new Restaurant { RestaurantName = name };
                restaurant.MainLocation = location;
                restaurant.IconName = "restaurant";

                //fav.CreateDate = DateTime.Now;
                //fav.FavUserID = user.FavUserID;
                //fav.RestaurantID = restaurant.RestaurantID;
                restaurant.MainLocation = location;
                restaurant.WebSiteURL = website;
                restaurant.CreateDate = DateTime.Now;
                perk.CreateDate = DateTime.Now;
                perk.PerkType = PerkType.AddedRestaurant;
                perk.FavUserID = user.FavUserID;
                perk.Value = 2;

                db.FavUsers.Attach(user);
                user.PerkTotal = user.PerkTotal + perk.Value;

                db.Locations.Add(location);
                db.Restaurants.Add(restaurant);
                //db.Favorites.Add(fav);
                db.Perks.Add(perk);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    e.ToString();
                }

            }
            return RedirectToAction("Details", new { id = restaurant.RestaurantID });
        }

        [HttpPost]
        public ActionResult ShowMenu(Restaurant rest)
        {
            if (ModelState.IsValid)
            {
                RestaurantViewClass RestaurantView;

                //Restaurant restaurant = db.Restaurants.Find(rest.RestaurantID);
                Restaurant restaurant = RestaurantMaster.GetRestaurantByID(rest.RestaurantID,db);

                FavUser user;

                user = FavUserMaster.GetCurrentUser(db);

                if (restaurant == null)
                {
                    return HttpNotFound();
                }

                try
                {
                    RestaurantView = new RestaurantViewClass { Restaurant = restaurant, FavUser = user };

                    return PartialView(RestaurantView);
                }
                catch
                {
                    return HttpNotFound();
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddNewFood(Restaurant rest, FoodItem food)
        {
            if (ModelState.IsValid)
            {
                food.RestaurantID = rest.RestaurantID;
                food.CreateDate = DateTime.Now;
                db.FoodItems.Add(food);
                db.SaveChanges();

                return PartialView(food);
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddNewDrink(Restaurant rest, DrinkItem drink)
        {
            if (ModelState.IsValid)
            {
                drink.RestaurantID = rest.RestaurantID;
                drink.CreateDate = DateTime.Now;
                db.DrinkItems.Add(drink);
                db.SaveChanges();

                return PartialView(drink);
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddNewEmployee(Restaurant rest, Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.RestaurantID = rest.RestaurantID;
                employee.CreateDate = DateTime.Now;
                db.Employee.Add(employee);
                db.SaveChanges();

                return PartialView(employee);
            }
            return null;
        }

       

        [HttpPost]
        public bool EditRestaurant(Restaurant rest, Location loc)
        {
            if (ModelState.IsValid)
            {
                if (rest.WebSiteURL != null && rest.WebSiteURL.Trim() != String.Empty)
                {
                    if (!rest.WebSiteURL.ToLower().StartsWith("http"))
                    {
                        rest.WebSiteURL = "http://" + rest.WebSiteURL;
                    }
                }

                db.Restaurants.Attach(rest);
                var entry = db.Entry(rest);
                entry.Property(e => e.RestaurantName).IsModified = true;
                entry.Property(e => e.WebSiteURL).IsModified = true;
                entry.Property(e => e.RestaurantType).IsModified = true;

                db.Locations.Attach(loc);
                var entry2 = db.Entry(loc);
                entry2.Property(f => f.AddressLn1).IsModified = true;
                entry2.Property(f => f.PhoneNo).IsModified = true;
                entry2.Property(f => f.City).IsModified = true;
                entry2.Property(f => f.State).IsModified = true;
                entry2.Property(f => f.ZipCode).IsModified = true;

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

                return false;
        }

        [HttpPost]
        public bool ToggleFoodLike(FoodContribution foodContributionUpdate)
        {
            if (ModelState.IsValid)
            {
                FavUser user;

                user = FavUserMaster.GetCurrentUser(db);

                FoodContribution food = db.FoodContributions.FirstOrDefault(a => a.FoodItemID == foodContributionUpdate.FoodItemID && a.FavUserID == user.FavUserID);

                if (food != null)
                {
                    food.LikeStatus = foodContributionUpdate.LikeStatus;
                        
                    db.FoodContributions.Attach(food);
                    var entry = db.Entry(food);
                    entry.Property(e => e.LikeStatus).IsModified = true;
                } else
                {
                    foodContributionUpdate.FavUserID = user.FavUserID;

                    db.FoodContributions.Add(foodContributionUpdate);
                }

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        [HttpPost]
        public bool ToggleDrinkLike(DrinkContribution drinkContributionUpdate)
        {
            if (ModelState.IsValid)
            {
                FavUser user;

                user = FavUserMaster.GetCurrentUser(db);

                DrinkContribution drink = db.DrinkContributions.FirstOrDefault(a => a.DrinkItemID == drinkContributionUpdate.DrinkItemID && a.FavUserID == user.FavUserID);

                if (drink != null)
                {
                    drink.LikeStatus = drinkContributionUpdate.LikeStatus;

                    db.DrinkContributions.Attach(drink);
                    var entry = db.Entry(drink);
                    entry.Property(e => e.LikeStatus).IsModified = true;
                }
                else
                {
                    drinkContributionUpdate.FavUserID = user.FavUserID;

                    db.DrinkContributions.Add(drinkContributionUpdate);
                }

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        [HttpPost]
        public bool ToggleEmployeeLike(EmployeeContribution employeeContributionUpdate)
        {
            if (ModelState.IsValid)
            {
                FavUser user;

                user = FavUserMaster.GetCurrentUser(db);

                EmployeeContribution employee = db.EmployeeContributions.FirstOrDefault(a => a.EmployeeID == employeeContributionUpdate.EmployeeID && a.FavUserID == user.FavUserID);

                if (employee != null)
                {
                    employee.LikeStatus = employeeContributionUpdate.LikeStatus;

                    db.EmployeeContributions.Attach(employee);
                    var entry = db.Entry(employee);
                    entry.Property(e => e.LikeStatus).IsModified = true;
                }
                else
                {
                    employeeContributionUpdate.FavUserID = user.FavUserID;

                    db.EmployeeContributions.Add(employeeContributionUpdate);
                }

                try
                {
                    db.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}