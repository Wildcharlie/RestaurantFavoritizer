using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class FoodContribution
    {
        public int FoodContributionID { get; set; }
        public int FavUserID { get; set; }
        public int RestaurantID { get; set; }
        public int FoodItemID { get; set; }
        public LikeStatus LikeStatus { get; set; }
    }

    public class DrinkContribution
    {
        public int DrinkContributionID { get; set; }
        public int FavUserID { get; set; }
        public int RestaurantID { get; set; }
        public int DrinkItemID  { get; set; }
        public LikeStatus LikeStatus { get; set; }
    }

    public class EmployeeContribution
    {
        public int EmployeeContributionID { get; set; }
        public int FavUserID { get; set; }
        public int RestaurantID { get; set; }
        public int EmployeeID { get; set; }
        public LikeStatus LikeStatus { get; set; }
    }

    public enum LikeStatus
    {
        Like,
        Dislike,
        None
    }
}