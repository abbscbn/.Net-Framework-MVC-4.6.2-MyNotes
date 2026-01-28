namespace MyNotes.DataAccessLayer.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ExpandPasswordLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn(
       "dbo.EverNoteUsers",
       "Password",
       c => c.String(nullable: false, maxLength: 256)
   );
        }

        public override void Down()
        {
            AlterColumn(
       "dbo.EverNoteUsers",
       "Password",
       c => c.String(nullable: false, maxLength: 50)
   );
        }
    }
}
