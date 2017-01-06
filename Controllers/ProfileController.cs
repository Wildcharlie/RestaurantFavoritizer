using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RestaurantFavoritizer.Models;
using RestaurantFavoritizer.DAL;

namespace RestaurantFavoritizer.Controllers
{
    public class ProfileController : Controller
    {
        private FavContext db = new FavContext();

        // GET: MyFavorites

        public ActionResult Index()
        {
            FavUser user = FavUserMaster.GetCurrentUser(db);
            ProfileViewClass pvc =  FavUserMaster.GetProfileViewClass(user, db);

            return View(pvc);
        }
        public ActionResult Details(int? id)
        {   
            FavUser user = FavUserMaster.GetFavUserByID(id,db);
            if (id != null && id == FavUserMaster.GetCurrentUser(db).FavUserID)
            {
                return RedirectToAction("Index");
            }            

            ProfileViewClass pvc = FavUserMaster.GetProfileViewClass(user,db);

            if (!user.isPublic)
            {
                //return RedirectToAction("Private");
                return View("Private", pvc);
            }

            return View(pvc);
        }

        public bool EditProfile(FavUser user, Location loc)
        {
            FavUser favUser = FavUserMaster.GetCurrentUser(db);
            bool success = false;

            if (user.FirstName != null && user.FirstName.Trim() != string.Empty)
            {
                favUser.FirstName = user.FirstName;
            }
            if (user.LastName != null &&  user.LastName.Trim() != string.Empty)
            {
                favUser.LastName = user.LastName;
            }
            if (user.Avatar != null &&  user.Avatar.Trim() != string.Empty)
            {
                favUser.Avatar = FavUserMaster.CreateMD5(user.Avatar).ToLower().Trim();
            }

            if (loc.City != null && loc.City.Trim() != string.Empty)
            {
                favUser.Location.City = loc.City.Trim();
            }

            if (loc.ZipCode != null && loc.City.Trim() != string.Empty)
            {
                favUser.Location.ZipCode = loc.ZipCode.Trim();
            }

            db.FavUsers.Attach(favUser);
            db.Entry(favUser).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            success = true;
            return success;
        }

        public string AddFriend(string txtEmail)
        {
            string returnVal;
            returnVal = FriendMaster.AddFriend(txtEmail, db);

            return returnVal;
        }
    }
}