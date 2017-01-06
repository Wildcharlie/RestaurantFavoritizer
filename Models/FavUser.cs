using RestaurantFavoritizer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class FavUser
    {
        public int FavUserID { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Location Location { get; set; }
        public bool isPublic { get; set; }
        public virtual ICollection<Favorite> Favorites { get; set; }
        public virtual ICollection<Perk> Perks { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<FoodContribution> FoodContributions { get; set; }
        public virtual ICollection<DrinkContribution> DrinkContributions { get; set; }
        public virtual ICollection<EmployeeContribution> EmployeeContributions { get; set; }
        public int PerkTotal { get; set; }
    }

    public static class FavUserMaster
    {
        public static FavUser GetCurrentUser(RestaurantFavoritizer.DAL.FavContext db)
        {
            string userName;
            FavUser user;

            userName = System.Web.HttpContext.Current.User.Identity.Name;
            user = db.FavUsers.SingleOrDefault(FavUser => FavUser.UserName == userName);

            if (user == null)
            {
                Location loc;
                loc = new Location { City = "Louisville", ZipCode = "40292" };
                user = new FavUser { FirstName = "Guest", Location = loc };
            }

            return user;
        }

        public static FavUser GetFavUserByName(string userName, FavContext db = null)
        {
            if (db == null)
            {
                db = new FavContext();
            }

            try
            {
                FavUser user = db.FavUsers.FirstOrDefault(FavUser => FavUser.UserName == userName);
                return user;
            }
            catch
            {
                return null;
            }
        }

        public static FavUser GetFavUserByID(int? id, FavContext db = null)
        {
            if (db == null)
            {
                db = new FavContext();
            }

            try
            {
                return db.FavUsers.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public static void CreateNewFavUser(ApplicationUser user, string firstName, string lastName, string city, string zip)
        {
            FavContext db;
            Location location;
            FavUser favUser;
            Perk p;

            db = new FavContext();
            
            location = new Location { };
            location.City = city;
            location.ZipCode = zip;
            location.State = "KY";
            location.Latitude = 38.2157f;
            location.Longitude = -85.7590743f;

            favUser = new FavUser { UserName = user.Email, CreateDate = DateTime.Now, Location = location, FirstName = firstName, LastName = lastName, isPublic = true, PerkTotal = 5};
            p = new Perk { CreateDate = DateTime.Now, FavUserID = favUser.FavUserID, PerkType = PerkType.CreatedAccount, Value = 5 };
            db.FavUsers.Add(favUser);
            db.Locations.Add(location);
            db.Perks.Add(p);
            db.SaveChanges();
        }

        public static string CreateMD5(string input)
        {
            input = input.Trim().ToLower();  
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static ProfileViewClass GetProfileViewClass(FavUser user,FavContext db)
        {
            ProfileViewClass pvc = new ProfileViewClass { FavUser = user };

            var perks = pvc.FavUser.Perks.OrderByDescending(p => p.CreateDate).Take(10);
            var favs = pvc.FavUser.Favorites.OrderByDescending(p => p.CreateDate).Take(10);
            var friendIDs = pvc.FavUser.Friends.Where(x => x.FavUserID == user.FavUserID).Select(x => x.FriendFavUserID);

            var friends = db.FavUsers.Where(x => friendIDs.Contains(x.FavUserID));

            if (perks == null)
            {
                pvc.RecentPerks = new Perk[] { };
            }
            else
            {
                pvc.RecentPerks = perks.ToArray();
            }

            if (perks == null)
            {
                pvc.RecentFavorites = new Favorite[] { };
            }
            else
            {
                pvc.RecentFavorites = favs.ToArray();
            }

            if (perks == null)
            {
                pvc.Friends = new FavUser[] { };
            }
            else
            {
                pvc.Friends = friends.ToArray();
            }

            return pvc;
        }
    }
    public class ProfileViewClass
    {
        public FavUser FavUser { get; set; }
        public int FavCount
        {
            get
            {
                return FavUser.Favorites == null ? 0 : FavUser.Favorites.Count();
            }
        }

        public string Title
        {
            get
            {
                return PerkMaster.FavTitle(FavUser.PerkTotal);
            }
        }
        public Perk[] RecentPerks { get; set; }
        public Favorite[] RecentFavorites { get; set; }

        public FavUser[] Friends { get; set; }
        public string AvatarURL
        {
            get
            {
                string returnVal;

                if (FavUser.Avatar == null || FavUser.Avatar == string.Empty)
                {
                    returnVal = "/Content/Images/defaultUserImage.png";
                }
                else
                {
                    returnVal = "http://www.gravatar.com/avatar/" + FavUser.Avatar;
                }

                return returnVal;
            }
        }
    }
}