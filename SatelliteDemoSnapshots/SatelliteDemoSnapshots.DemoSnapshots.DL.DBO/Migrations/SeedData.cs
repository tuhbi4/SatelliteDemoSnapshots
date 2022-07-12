using FluentMigrator;
using SatelliteDemoSnapshots.DemoSnapshots.Common.Entities;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DBO.Migrations
{
    [Migration(2)]
    public class SeedData : Migration
    {
        public override void Up()
        {
            Insert.IntoTable("DemoSnapshots").Row(new
            {
                Satellite = Satellites.Kanopus,
                ShootingDate = new System.DateTime(2022, 01, 10),
                Cloudiness = 50,
                Coordinates = "POLYGON((3.6718750000000044 25.73989230448949,14.921875000000004 25.73989230448949,14.921875000000004 18.916011030403887,3.6718750000000044 18.916011030403887,3.6718750000000044 25.73989230448949))"
            });
            Insert.IntoTable("DemoSnapshots").Row(new
            {
                Satellite = Satellites.BS,
                ShootingDate = new System.DateTime(2021, 02, 11),
                Cloudiness = 10,
                Coordinates = "POLYGON((35.23238462584629 57.60906073517954,35.54000181334629 56.16893557803185,37.03414243834629 55.972705249882125,37.95699400084629 57.30173453950533,35.23238462584629 57.60906073517954))"
            });
        }

        public override void Down()
        {
            //empty, not using
        }
    }
}