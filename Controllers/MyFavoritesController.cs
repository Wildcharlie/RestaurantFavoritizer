using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantFavoritizer.Models;
using RestaurantFavoritizer.DAL;

namespace RestaurantFavoritizer.Controllers
{
    public class MyFavoritesController : Controller
    {
        private FavContext db = new FavContext();

        // GET: MyFavorites

        public ActionResult Index()
        {
            FavUser user = FavUserMaster.GetCurrentUser(db);
            MainViewClass cls;

            cls = new MainViewClass { Restaurants = new List<Restaurant> { }, FavUser = user };
            return View(cls);
        }

        [HttpPost]
        public ActionResult SearchRestaurant(string SearchString)
        {
            MainViewClass RestaurantView;
            FavUser user;
            db.Configuration.ProxyCreationEnabled = false;

            user = FavUserMaster.GetCurrentUser(db);

            var rests = from m in db.Restaurants
                        select m;

            if (!String.IsNullOrEmpty(SearchString))
            {
                rests = rests.Where(s => s.RestaurantName.Contains(SearchString));
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

            return PartialView(RestaurantView);
        }

        [HttpPost]
        public ActionResult AddFavoriteRestaurant(Restaurant rest, FavUser user)
        {
            if (ModelState.IsValid)
            {
                var favorite = from m in db.Favorites
                               where m.FavUserID == user.FavUserID
                               && m.RestaurantID == rest.RestaurantID
                               select m;

                if (!favorite.Any())
                {
                    Perk p;
                    p = new Perk();
                    p.CreateDate = DateTime.Now;
                    p.FavUserID = user.FavUserID;
                    p.PerkType = PerkType.AddedMyFavorite;
                    p.Value = 10;

                    Favorite fav;
                    fav = new Favorite();

                    fav.FavUserID = user.FavUserID;
                    fav.RestaurantID = rest.RestaurantID;
                    fav.CreateDate = DateTime.Now;
                    db.Favorites.Add(fav);

                    try
                    {
                        db.SaveChanges();
                        return PartialView(rest);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else return null;
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddFavoriteFood(Restaurant rest, FavUser user, FoodItem food)
        {
            if (ModelState.IsValid)
            {


                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                if (favorite.Count > 0)
                {
                    foreach (var item in favorite)
                    {
                        item.FoodItemID = food.FoodItemID;
                        db.Favorites.Attach(item);
                        var entry = db.Entry(item);
                        entry.Property(e => e.FoodItemID).IsModified = true;
                    }

                    try
                    {
                        db.SaveChanges();
                        return PartialView(food);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }


        [HttpPost]
        public ActionResult AddFavoriteDrink(Restaurant rest, FavUser user, DrinkItem drink)
        {
            if (ModelState.IsValid)
            {


                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                if (favorite.Count > 0)
                {
                    foreach (var item in favorite)
                    {
                        item.DrinkItemID = drink.DrinkItemID;
                        db.Favorites.Attach(item);
                        var entry = db.Entry(item);
                        entry.Property(e => e.DrinkItemID).IsModified = true;
                    }

                    try
                    {
                        db.SaveChanges();
                        return PartialView(drink);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult AddFavoriteEmployee(Restaurant rest, FavUser user, Employee employee)
        {
            if (ModelState.IsValid)
            {


                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                if (favorite.Count > 0)
                {
                    foreach (var item in favorite)
                    {
                        item.EmployeeID = employee.EmployeeID;
                        db.Favorites.Attach(item);
                        var entry = db.Entry(item);
                        entry.Property(e => e.EmployeeID).IsModified = true;
                    }

                    try
                    {
                        db.SaveChanges();
                        return PartialView(employee);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        [HttpPost]
        public bool RemoveFavoriteRestaurant(Restaurant rest, FavUser user)
        {
            if (ModelState.IsValid)
            {
                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                foreach (var item in favorite)
                {
                    db.Favorites.Remove(item);
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
        public bool RemoveFavoriteFood(Restaurant rest, FavUser user)
        {
            if (ModelState.IsValid)
            {


                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                foreach (var item in favorite)
                {
                    item.FoodItemID = null;
                    db.Favorites.Attach(item);
                    var entry = db.Entry(item);
                    entry.Property(e => e.FoodItemID).IsModified = true;
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
        public bool RemoveFavoriteDrink(Restaurant rest, FavUser user)
        {
            if (ModelState.IsValid)
            {


                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                foreach (var item in favorite)
                {
                    item.DrinkItemID = null;
                    db.Favorites.Attach(item);
                    var entry = db.Entry(item);
                    entry.Property(e => e.DrinkItemID).IsModified = true;
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
        public bool RemoveFavoriteEmployee(Restaurant rest, FavUser user)
        {
            if (ModelState.IsValid)
            {


                var fav = from m in db.Favorites
                          where m.FavUserID == user.FavUserID
                          && m.RestaurantID == rest.RestaurantID
                          select m;

                List<Favorite> favorite;
                favorite = new List<Favorite> { };
                favorite = fav.ToList();

                foreach (var item in favorite)
                {
                    item.EmployeeID = null;
                    db.Favorites.Attach(item);
                    var entry = db.Entry(item);
                    entry.Property(e => e.EmployeeID).IsModified = true;
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
        public bool TogglePrivacy(FavUser user)
        {
            if (ModelState.IsValid)
            {
                FavUser userUpdate = db.FavUsers.First(a => a.FavUserID == user.FavUserID);

                userUpdate.isPublic = user.isPublic;
                db.FavUsers.Attach(userUpdate);
                var entry = db.Entry(userUpdate);
                entry.Property(e => e.isPublic).IsModified = true;

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