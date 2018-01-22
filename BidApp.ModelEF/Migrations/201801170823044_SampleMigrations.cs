namespace BidApp.ModelEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SampleMigrations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        BidId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Status = c.String(),
                        Time = c.DateTime(nullable: false),
                        TimeEnd = c.DateTime(nullable: false),
                        Returned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BidId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        To = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        BidId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Bids", t => t.BidId, cascadeDelete: true)
                .Index(t => t.BidId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "BidId", "dbo.Bids");
            DropIndex("dbo.Comments", new[] { "BidId" });
            DropTable("dbo.Comments");
            DropTable("dbo.Bids");
        }
    }
}
