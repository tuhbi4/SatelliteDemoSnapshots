using FluentMigrator;

namespace SatelliteDemoSnapshots.DemoSnapshots.DL.DBO.Migrations
{
    public partial class CreateTable
    {
        [Migration(3)]
        public class AddRow : Migration
        {
            public override void Up()
            {
                Alter.Table("DemoSnapshots").AddColumn("Turn").AsInt32().NotNullable().WithDefaultValue(1);
                Update.Table("DemoSnapshots").Set(new { Turn = 2 }).Where(new { Id = 1 });
                Update.Table("DemoSnapshots").Set(new { Turn = 5 }).Where(new { Id = 2 });
            }

            public override void Down()
            {
                Delete.Column("Turn").FromTable("DemoSnapshots");
            }
        }
    }
}