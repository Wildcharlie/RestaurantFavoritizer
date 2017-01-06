using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantFavoritizer.Models
{
    public class Perk
    {
        public int PerkID { get; set; }
        public int FavUserID { get; set; }
        public int Value { get; set; }
        public DateTime CreateDate { get; set; }
        public PerkType PerkType { get; set; }
    }

    public enum PerkType
    {
        [Display(Name = "Created Account")]
        CreatedAccount,
        [Display(Name = "Added Restaurant")]
        AddedRestaurant,
        [Display(Name = "Added Food Item")]
        AddedFoodItem,
        [Display(Name = "Added Drink Item")]
        AddedDrinkItem,
        [Display(Name = "Added Opinion")]
        AddedOpinion,
        [Display(Name = "Added Favorite")]
        AddedMyFavorite,
        [Display(Name = "Added Friend")]
        AddedFriend,
        [Display(Name = "Frequent Favorite")]
        FrequentFavorite,
        [Display(Name = "Connoisseur of Concoctions")]
        ConnoisseurofConcoctions,
    }

    public enum Title
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Noob of Nosh")]
        NoobOfNosh =15,
        [Display(Name = "Green Tongue")]
        GreenTongue = 25,
        [Display(Name = "Casual Eater")]
        CasualEater = 50,
        [Display(Name = "Grub Consultant")]
        GrubConsultant = 200,
        [Display(Name = "Food Snob")]
        FoodSnob = 1000,
        [Display(Name = "Master of Munchies")]
        MasterOfMunchies = 5000,
        [Display(Name = "God of All Grub")]
        GodOfAllGrub = 10000,
    }

    public static class PerkMaster
    {
        public static string FavTitle(int points)
        {

            if(points < (int)Title.NoobOfNosh)
            {
                return EnumNinja.DisplayName(Title.None);
            }
            if (points < (int)Title.GreenTongue)
            {
                return EnumNinja.DisplayName(Title.NoobOfNosh);
            }
            if (points < (int)Title.CasualEater)
            {
                return EnumNinja.DisplayName(Title.GreenTongue);
            }
            if (points < (int)Title.GrubConsultant)
            {
                return EnumNinja.DisplayName(Title.CasualEater);
            }
            if (points < (int)Title.FoodSnob)
            {
                return EnumNinja.DisplayName(Title.GrubConsultant);
            }
            if (points < (int)Title.MasterOfMunchies)
            {
                return EnumNinja.DisplayName(Title.FoodSnob);
            }
            if (points < (int)Title.GodOfAllGrub)
            {
                return EnumNinja.DisplayName(Title.MasterOfMunchies);
            }
            else
            {
                return EnumNinja.DisplayName(Title.GodOfAllGrub);
            }

        }
    }

}