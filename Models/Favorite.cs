using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class Favorite
    {
        public int FavoriteID { get; set; }
        public int FavUserID { get; set; }
        public int RestaurantID { get; set; }
        public int? FoodItemID { get; set; }
        public int? DrinkItemID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual FoodItem FoodItem { get; set; }
        public virtual DrinkItem DrinkItem { get; set; }
        public virtual Employee Employee { get; set; }


    }
}