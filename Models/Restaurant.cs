using RestaurantFavoritizer.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class Restaurant
    {
        public int RestaurantID { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(100)]
        public string RestaurantName { get; set; }
        public string WebSiteURL { get; set; }
        public string IconName { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Location MainLocation { get; set; }
        public virtual RestaurantType RestaurantType { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<FoodItem> FoodItems { get; set; }
        public virtual ICollection<DrinkItem> DrinkItems { get; set; }
        [NotMapped]
        public int FavCount { get; set; }
    }

    public class Employee
    {
        public int EmployeeID { get; set; }
        [Index("EmployeeRestIndex", 1, IsUnique = true)]
        [MaxLength(40)]
        public string EmployeeName { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public string EmployeeDescription { get; set; }
        public DateTime CreateDate { get; set; }
        [Index("EmployeeRestIndex", 2, IsUnique = true)]
        public int RestaurantID { get; set; }
        public virtual Location CurrentLocation { get; set; }
        [NotMapped]
        public int LikeCount { get; set; }
        [NotMapped]
        public int DisLikeCount { get; set; }
    }

    public class FoodItem
    {
        public int FoodItemID { get; set; }
        [Index("FoodRestIndex", 1, IsUnique = true)]
        [MaxLength(40)]
        public string FoodItemName { get; set; }
        public FoodItemType FoodItemType { get; set; }
        public string FoodItemDescription { get; set; }
        public float price { get; set; }
        public DateTime CreateDate { get; set; }
        [Index("FoodRestIndex", 2, IsUnique = true)]
        public int RestaurantID { get; set; }
        [NotMapped]
        public int LikeCount { get; set; }
        [NotMapped]
        public int DisLikeCount { get; set; }
    }

    public enum EmployeeType
    {
        [Display(Name = "Server")]
        Server,
        [Display(Name = "Bartender")]
        Bartender,
        [Display(Name = "Manager")]
        Manager,
        [Display(Name = "Other")]
        Other,
    }

    public enum FoodItemType
    {
        [Display(Name = "Appetizer")]
        Appetizer,
        [Display(Name = "Entree")]
        Entree,
        [Display(Name = "Snack")]
        Snack,
        [Display(Name = "Dessert")]
        Dessert,
    }

    public class DrinkItem
    {
        public int DrinkItemID { get; set; }
        [Index("DrinkRestIndex", 1, IsUnique = true)]
        [MaxLength(40)]
        public string DrinkItemName { get; set; }
        public DrinkItemType DrinkItemType { get; set; }
        public string DrinkItemDescription { get; set; }
        public float price { get; set; }
        public DateTime CreateDate { get; set; }
        [Index("DrinkRestIndex", 2, IsUnique = true)]
        public int RestaurantID { get; set; }
        [NotMapped]
        public int LikeCount { get; set; }
        [NotMapped]
        public int DisLikeCount { get; set; }
    }

    public enum DrinkItemType
    {
        [Display(Name = "Beverage")]
        Beverage,
        [Display(Name = "Cocktail")]
        Cocktail,
        [Display(Name = "Shooter")]
        Shooter,
        [Display(Name = "Frozen")]
        Frozen,
        [Display(Name = "Martini")]
        Martini,
        [Display(Name = "Beer")]
        Beer,
        [Display(Name = "Wine")]
        Wine,
        [Display(Name = "Spirit")]
        Spirit,
    }

    public enum RestaurantType
    {
        [Display(Name = "Casual Dining")]
        Casual,
        [Display(Name = "Fine Dining")]
        FineDining,
        [Display(Name = "Fast Food")]
        FastFood,
        [Display(Name = "Ethnic Cuisine")]
        Ethnic,
        [Display(Name = "Other Food")]
        Other,
    }

    public class MainViewClass
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public FavUser FavUser { get; set; }
        public RestaurantType RestaurantType { get; set; }
        public FriendResult[] FriendResults { get; set; }
        public int FavCount
        {
            get
            {
                return FavUser.Favorites == null ? 0 : FavUser.Favorites.Count();
            }
        }

        [EmailAddress]
        [Display(Name = "Gravitar Email")]
        public string GravitarEmail { get; set; }

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

    public class RestaurantViewClass
    {
        public Restaurant Restaurant { get; set; }
        public FavUser FavUser { get; set; }
        public Favorite Favorite { get; set; }
        public RestaurantType RestaurantType { get; set; }

        public FoodItemType FoodItemType;
        public DrinkItemType DrinkItemType;
        public EmployeeType EmployeeType;

        public string PlainPhoneNo
        {
            get
            {
                if (Restaurant.MainLocation.PhoneNo == null)
                {
                    return String.Empty;
                }
                else
                {
                    Regex digitsOnly = new Regex(@"[^\d]");
                    return digitsOnly.Replace(Restaurant.MainLocation.PhoneNo, "");
                }
            }
        }
        public string FormattedPhoneNo
        {
            get { return this.Restaurant.MainLocation.PhoneNo; }
        }
        public string GooglePlaceURL
        {
            get
            {
                if (this.Restaurant.MainLocation.Annotation == null)
                {
                    return "#";
                }
                else
                {
                    return this.Restaurant.MainLocation.Annotation;
                }
            }
        }
        public string Address
        {
            get
            {
                if (this.Restaurant.MainLocation.AddressLn1 == null)
                {
                    return String.Empty;
                }
                return this.Restaurant.MainLocation.AddressLn1;
            }
        }
        public int UserID
        {
            get { return this.FavUser.FavUserID; }
        }
        public string RestType
        {
            get { return EnumNinja.DisplayName(this.Restaurant.RestaurantType); }
        }
    }

    public class MainMapViewClass
    {
        public IEnumerable<Restaurant> Restaurants { get; set; }
        public FavUser FavUser { get; set; }
        public RestaurantType? RestaurantType { get; set; }

    }

    public static class RestaurantMaster
    {
        public static string FormatContentString(Restaurant rest)
        {
            StringBuilder builder;

            builder = new StringBuilder();

            builder.Append("<div> class='map-infocontent' style='' ");

            builder.Append("</div>  ");

            return builder.ToString();
        }

        public static MainViewClass GetMainViewClass(FavContext db)
        {
            FavUser user;
            MainViewClass cls;
            string sql;

            user = FavUserMaster.GetCurrentUser(db);

            try
            {
                var restaurants = from r in db.Restaurants
                                  where r.MainLocation.City == user.Location.City
                                  select r;

                restaurants = restaurants.OrderByDescending(s => s.CreateDate).Take(15);
                cls = new MainViewClass { Restaurants = restaurants.ToArray(), FavUser = user };

                sql = "SELECT f.FavUserID, f.FirstName, f.LastName,p.PerkType, p.Value, P.CreateDate " + Environment.NewLine
                    + " FROM dbo.Perk p JOIN dbo.FavUser f ON f.FavUserID = p.FavUserID " + Environment.NewLine
                    + " WHERE f.FavUserID IN(SELECT FriendFavUserID FROM dbo.Friend WHERE FavUserID = '" + user.FavUserID + "') " + Environment.NewLine
                    + " ORDER BY p.CreateDate Desc " + Environment.NewLine
                    + " OFFSET 0 ROWS "
                    + " FETCH NEXT 10 ROWS ONLY;";

                cls.FriendResults = db.Database.SqlQuery<FriendResult>(sql).ToArray();
            }
            catch (Exception e)
            {
                e.ToString();
                cls = new MainViewClass { Restaurants = new List<Restaurant> { }, FavUser = user };
            }
            return cls;
        }

        public static Restaurant GetRestaurantByID(int? restID, FavContext db)
        {
            if (restID != null)
            {

                Restaurant restaurant = db.Restaurants.Find(restID);

                var _foodlikes = db.FoodContributions.Where(m => m.RestaurantID == restaurant.RestaurantID && m.LikeStatus == LikeStatus.Like)
            .GroupBy(m => m.FoodItemID).ToDictionary(d => d.Key, d => d.Count());

                var _fooddislikes = db.FoodContributions.Where(m => m.RestaurantID == restaurant.RestaurantID && m.LikeStatus == LikeStatus.Dislike)
            .GroupBy(m => m.FoodItemID).ToDictionary(d => d.Key, d => d.Count());

                var _drinklikes = db.DrinkContributions.Where(m => m.RestaurantID == restaurant.RestaurantID && m.LikeStatus == LikeStatus.Like)
            .GroupBy(m => m.DrinkItemID).ToDictionary(d => d.Key, d => d.Count());

                var _drinkdislikes = db.DrinkContributions.Where(m => m.RestaurantID == restaurant.RestaurantID && m.LikeStatus == LikeStatus.Dislike)
            .GroupBy(m => m.DrinkItemID).ToDictionary(d => d.Key, d => d.Count());

                var _emplikes = db.EmployeeContributions.Where(m => m.RestaurantID == restaurant.RestaurantID && m.LikeStatus == LikeStatus.Like)
            .GroupBy(m => m.EmployeeID).ToDictionary(d => d.Key, d => d.Count());

                var _empdislikes = db.EmployeeContributions.Where(m => m.RestaurantID == restaurant.RestaurantID && m.LikeStatus == LikeStatus.Dislike)
            .GroupBy(m => m.EmployeeID).ToDictionary(d => d.Key, d => d.Count());



                foreach (FoodItem f in restaurant.FoodItems)
                {
                    f.LikeCount = (_foodlikes.ContainsKey(f.FoodItemID)) ? _foodlikes[f.FoodItemID] : 0;
                    f.DisLikeCount = (_fooddislikes.ContainsKey(f.FoodItemID)) ? _fooddislikes[f.FoodItemID] : 0;
                }

                foreach (DrinkItem d in restaurant.DrinkItems)
                {
                    d.LikeCount = (_drinklikes.ContainsKey(d.DrinkItemID)) ? _drinklikes[d.DrinkItemID] : 0;
                    d.DisLikeCount = (_drinkdislikes.ContainsKey(d.DrinkItemID)) ? _drinkdislikes[d.DrinkItemID] : 0;
                }

                foreach (Employee e in restaurant.Employees)
                {
                    e.LikeCount = (_emplikes.ContainsKey(e.EmployeeID)) ? _emplikes[e.EmployeeID] : 0;
                    e.DisLikeCount = (_empdislikes.ContainsKey(e.EmployeeID)) ? _empdislikes[e.EmployeeID] : 0;
                }

                return restaurant;
            }
            else
            {
                return null;
            }
        }
    }

    public class FriendResult
    {
        public int FavUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public PerkType PerkType { get; set; }
        public int Value { get; set; }
        public DateTime CreateDate { get; set; }
    }
}