using FluentMigrator;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DBO.Migrations
{
    [Migration(1)]
    public partial class CreateTable : Migration
    {
        public override void Up()
        {
            Create.Table("DemoSnapshots")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Satellite").AsString(255).NotNullable()
                .WithColumn("ShootingDate").AsDateTime().NotNullable()
                .WithColumn("Cloudiness").AsDecimal(5, 2).Nullable()
                .WithColumn("Coordinates").AsCustom("GEOGRAPHY").NotNullable();
        }

        public override void Down()
        {
            Delete.Table("DemoSnapshots");
        }
    }
}