namespace MyNotes.DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 150),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        Text = c.String(nullable: false, maxLength: 2000),
                        IsDraft = c.Boolean(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        NoteImageFilename = c.String(maxLength: 30),
                        CategoryId = c.Int(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.EverNoteUsers", t => t.OwnerId, cascadeDelete: true)
                .Index(t => t.CategoryId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false, maxLength: 250),
                        OwnerId = c.Int(nullable: false),
                        NoteId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notes", t => t.NoteId, cascadeDelete: true)
                .ForeignKey("dbo.EverNoteUsers", t => t.OwnerId)
                .Index(t => t.OwnerId)
                .Index(t => t.NoteId);
            
            CreateTable(
                "dbo.EverNoteUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 25),
                        Surname = c.String(nullable: false, maxLength: 25),
                        Email = c.String(nullable: false, maxLength: 70),
                        Password = c.String(nullable: false, maxLength: 50),
                        Username = c.String(nullable: false, maxLength: 30),
                        ProfileImageFilename = c.String(maxLength: 30),
                        IsActive = c.Boolean(nullable: false),
                        IsAdmin = c.Boolean(nullable: false),
                        ActiveGuid = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifedOn = c.DateTime(nullable: false),
                        ModifiedUsername = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NoteId = c.Int(nullable: false),
                        LikedUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EverNoteUsers", t => t.LikedUserId)
                .ForeignKey("dbo.Notes", t => t.NoteId, cascadeDelete: true)
                .Index(t => t.NoteId)
                .Index(t => t.LikedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notes", "OwnerId", "dbo.EverNoteUsers");
            DropForeignKey("dbo.Comments", "OwnerId", "dbo.EverNoteUsers");
            DropForeignKey("dbo.Likes", "NoteId", "dbo.Notes");
            DropForeignKey("dbo.Likes", "LikedUserId", "dbo.EverNoteUsers");
            DropForeignKey("dbo.Comments", "NoteId", "dbo.Notes");
            DropForeignKey("dbo.Notes", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Likes", new[] { "LikedUserId" });
            DropIndex("dbo.Likes", new[] { "NoteId" });
            DropIndex("dbo.Comments", new[] { "NoteId" });
            DropIndex("dbo.Comments", new[] { "OwnerId" });
            DropIndex("dbo.Notes", new[] { "OwnerId" });
            DropIndex("dbo.Notes", new[] { "CategoryId" });
            DropTable("dbo.Likes");
            DropTable("dbo.EverNoteUsers");
            DropTable("dbo.Comments");
            DropTable("dbo.Notes");
            DropTable("dbo.Categories");
        }
    }
}
