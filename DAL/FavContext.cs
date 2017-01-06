using System;
using System.Collections.Generic;
using RestaurantFavoritizer.Models;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RestaurantFavoritizer.DAL
{
    public class FavContext : DbContext
    {
        public FavContext() : base("FavoritizerConnection")
        {
        }

        public DbSet<FavUser> FavUsers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Perk> Perks { get; set; }
        public DbSet<FoodContribution> FoodContributions { get; set; }
        public DbSet<DrinkContribution> DrinkContributions { get; set; }
        public DbSet<EmployeeContribution> EmployeeContributions { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<DrinkItem> DrinkItems { get; set; }
        public DbSet<Friend> Friends { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }

    }
}