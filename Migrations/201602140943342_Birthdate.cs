namespace RestaurantFavoritizer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Birthdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "Alias", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Alias");
            DropColumn("dbo.AspNetUsers", "BirthDate");
        }
    }
}
