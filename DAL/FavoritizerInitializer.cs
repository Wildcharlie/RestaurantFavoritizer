using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using RestaurantFavoritizer.Models;

namespace RestaurantFavoritizer.DAL
{
    public class FavoritizerInitializer : DropCreateDatabaseIfModelChanges<FavContext>
    {
        protected override void Seed(FavContext context)
        {
            var restaurants = new List<Restaurant>
            {
            new Restaurant{ RestaurantName="Test Cafe"}
            };

          //  restaurants.ForEach(s => context.Restaurants.Add(s));
          //  context.SaveChanges();
        }
    }
}