using MyNotes.Entities;
using System.Data.Entity;

namespace MyNotes.DataAccessLayer
{
    public class DatabaseContext : DbContext
    {


        // Veritabanı bağlantı dizesi adı
        public DbSet<Category> Categories { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<EverNoteUser> Users { get; set; }

        public DbSet<Liked> Likes { get; set; }

        public DbSet<Note> Notes { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // Like → Note (cascade kapalı)
            modelBuilder.Entity<Liked>()
                .HasRequired(l => l.Note)
                .WithMany(n => n.Likes)
                .HasForeignKey(l => l.NoteId)
                .WillCascadeOnDelete(false);

            // Like → User (cascade kapalı)
            modelBuilder.Entity<Liked>()
                .HasRequired(l => l.LikedUser)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.LikedUserId)
                .WillCascadeOnDelete(false);

            // Comment → Note
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Note)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NoteId)
                .WillCascadeOnDelete(false);

            // Comment → User
            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Owner)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.OwnerId)
                .WillCascadeOnDelete(false);

            // Note → User
            modelBuilder.Entity<Note>()
                .HasRequired(n => n.Owner)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.OwnerId)
                .WillCascadeOnDelete(false);

            // Note → Category
            modelBuilder.Entity<Note>()
                .HasRequired(n => n.Category)
                .WithMany(c => c.Notes)
                .HasForeignKey(n => n.CategoryId)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
            // İlişkileri ve diğer yapılandırmaları burada tanımlayabilirsiniz
        }

    }
}
