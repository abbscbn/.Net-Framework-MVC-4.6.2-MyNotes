using MyNotes.Entities;
using System.Data.Entity;

namespace MyNotes.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {


        // Veritabanı bağlantı dizesi adı
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<EverNoteUser> EvernoteUsers { get; set; }

        public DbSet<Liked> Likes { get; set; }

        public DbSet<Note> Notes { get; set; }


        public DatabaseContext() : base("DatabaseContext")
        {

            //Database.SetInitializer(new MyInitalizer());
        }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User -> Notes (OK)
            modelBuilder.Entity<Note>()
                .HasRequired(n => n.Owner)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.OwnerId)
                .WillCascadeOnDelete(true);

            // User -> Comments (NO CASCADE)
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Owner)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.OwnerId)
                .WillCascadeOnDelete(false);

            // User -> Likes (NO CASCADE)
            modelBuilder.Entity<Liked>()
                .HasRequired(l => l.LikedUser)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.LikedUserId)
                .WillCascadeOnDelete(false);

            // Category -> Notes
            modelBuilder.Entity<Note>()
                .HasRequired(n => n.Category)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.CategoryId)
                .WillCascadeOnDelete(true);

            // Note -> Comments
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Note)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NoteId)
                .WillCascadeOnDelete(true);

            // Note -> Likes
            modelBuilder.Entity<Liked>()
                .HasRequired(l => l.Note)
                .WithMany(n => n.Likes)
                .HasForeignKey(l => l.NoteId)
                .WillCascadeOnDelete(true);
        }




    }
}
