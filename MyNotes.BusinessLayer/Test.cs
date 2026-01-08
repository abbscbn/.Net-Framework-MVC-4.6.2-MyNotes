namespace MyNotes.BusinessLayer
{
    public class Test
    {

        //private Repository<EverNoteUser> repo_user = new Repository<EverNoteUser>();

        //private Repository<Category> repo_category = new Repository<Category>();

        //private Repository<Note> repo_note = new Repository<Note>();

        //private Repository<Comment> repo_comment = new Repository<Comment>();

        //public Test()
        //{
        //    using (var db = new DataAccessLayer.EntityFramework.DatabaseContext())
        //    {
        //        db.Database.Initialize(true);
        //    }
        //}

        //public void InsertTest()
        //{
        //    EverNoteUser user = new EverNoteUser()
        //    {
        //        Name = "Test01",
        //        Surname = "test",
        //        Email = "test@gmail.com",
        //        Password = "1",
        //        Username = "test01",
        //        IsActive = true,
        //        IsAdmin = false,
        //        ActiveGuid = Guid.NewGuid(),
        //        CreatedOn = DateTime.Now,
        //        ModifedOn = DateTime.Now.AddMinutes(5),
        //        ModifiedUsername = "test"

        //    };
        //    repo_user.Insert(user);

        //}

        //public void UpdateTest()
        //{
        //    EverNoteUser founded_user = repo_user.Find(x => x.Username == "test01");

        //    founded_user.Name = "UpdatedName";
        //    founded_user.Surname = "UpdatedSurname";
        //    repo_user.Update(founded_user);
        //}

        //public void DeleteTest()
        //{
        //    EverNoteUser founded_user = repo_user.Find(x => x.Name == "UpdatedName");
        //    repo_user.Delete(founded_user);
        //}

        //public void CommentTest()
        //{
        //    EverNoteUser founded_user = repo_user.Find(x => x.Id == 10);
        //    Note founded_note = repo_note.Find(x => x.Id == 19);

        //    Comment comment = new Comment()
        //    {
        //        Text = "Bu bir test yorumudur.",
        //        CreatedOn = DateTime.Now,
        //        ModifedOn = DateTime.Now,
        //        ModifiedUsername = founded_user.Username,
        //        Note = founded_note,
        //        Owner = founded_user
        //    };

        //    repo_comment.Insert(comment);

        //}

        //public void CategoryTest()
        //{
        //    // bilerek hangi hatayı alacağımızı görmek için Id'yi 1 verdik.
        //    repo_category.Delete(repo_category.Find(x => x.Id == 1));
        //}
    }
}

