using FluentMigrator;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DBO.Migrations
{
    [Migration(2)]
    public class SeedData : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("DemoSnapshots").Row(new
            {
                Satellite = "Canopus",
                ShootingDate = new System.DateTime(2022, 01, 10),
                Cloudiness = 50,
                Coordinates = "POLYGON1"
            });
            Insert.IntoTable("DemoSnapshots").Row(new
            {
                Satellite = "BKA",
                ShootingDate = new System.DateTime(2021, 02, 11),
                Cloudiness = 10,
                Coordinates = "POLYGON2"
            });
        }

        public override void Down()
        {
            //empty, not using
        }
    }
}