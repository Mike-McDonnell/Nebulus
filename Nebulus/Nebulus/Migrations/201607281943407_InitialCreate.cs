namespace Nebulus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MessageItems",
                c => new
                    {
                        MessageItemId = c.String(nullable: false, maxLength: 128),
                        MessageTitle = c.String(nullable: false),
                        MessageBody = c.String(nullable: false, maxLength: 2304),
                        MessageType = c.Int(nullable: false),
                        MessageLocation = c.Int(nullable: false),
                        MessagePriority = c.Int(nullable: false),
                        ScheduleStart = c.DateTimeOffset(nullable: false, precision: 7),
                        ScheduleInterval = c.Int(nullable: false),
                        SentTime = c.DateTimeOffset(nullable: false, precision: 7),
                        Creator = c.String(),
                        duration = c.Double(nullable: false),
                        Expiration = c.DateTimeOffset(nullable: false, precision: 7),
                        TargetGroup = c.String(),
                        MessageHeight = c.String(),
                        MessageWidth = c.String(),
                        MessageTop = c.String(),
                        MessageLeft = c.String(),
                        ADGroupTags = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MessageItemId);
            
            CreateTable(
                "dbo.PrintServiceSettingsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PrintServerNames = c.String(),
                        printServerServiceAccount = c.String(),
                        printServerServiceAccountPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ScreenSaverItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Modified = c.DateTimeOffset(nullable: false, precision: 7),
                        Name = c.String(),
                        CreatorId = c.String(),
                        Active = c.Boolean(nullable: false),
                        SlidesPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ScreenSaverItems");
            DropTable("dbo.PrintServiceSettingsModels");
            DropTable("dbo.MessageItems");
        }
    }
}
