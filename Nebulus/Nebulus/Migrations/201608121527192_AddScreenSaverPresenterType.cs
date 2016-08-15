namespace Nebulus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddScreenSaverPresenterType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScreenSaverItems", "Presenter", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScreenSaverItems", "Presenter");
        }
    }
}
