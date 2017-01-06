using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class Opinion
    {
        public int OpinionID { get; set; }
        public int FavUserID { get; set; }
        public string OpinionString { get; set; }
        public DateTime CreateDate { get; set; }
        public int? RestaurantID { get; set; }
        public int? FoodItemID { get; set; }
        public int? DrinkItemID { get; set; }
        public int? EmployeeID { get; set; }
        public int? LikeCount { get; set; }
    }
}